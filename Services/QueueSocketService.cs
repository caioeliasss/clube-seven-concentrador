using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using SocketIOClient;
using SocketIOClient.Exceptions;
using SevenConcentradorBridge.Models;

namespace SevenConcentradorBridge.Services;

// Documento da fila do servidor Clube Seven (guides/integracao-socket.md):
// { "_id", "action", "data", "status", "createdAt" }.
public class FilaDoc
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("action")]
    public string Action { get; set; } = "";

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}

// data { bico: "01", price: "5490" } — price em milésimos (C_SetPrice 4 dígitos × 1000).
// Bico/price aceitam string OU número: o backend já mandou price: 5490 (Number), e o
// STJ não converte Number→string por padrão (JsonException no Executar).
public class AlterarPrecoData
{
    [JsonPropertyName("bico")]
    [JsonConverter(typeof(JsonStringFlexivel))]
    public string? Bico { get; set; }

    [JsonPropertyName("price")]
    [JsonConverter(typeof(JsonStringFlexivel))]
    public string? Price { get; set; }
}

// data { bico, intervalMs } — equivalente ao antigo GET /visualizacao/stream.
public class VisualizarStreamData
{
    [JsonPropertyName("bico")]
    [JsonConverter(typeof(JsonStringFlexivel))]
    public string? Bico { get; set; }

    [JsonPropertyName("intervalMs")]
    [JsonConverter(typeof(JsonIntFlexivel))]
    public int? IntervalMs { get; set; }
}

// String que aceita JsonTokenType.String OU Number (5490 → "5490"; 5.49 → "5.49").
public sealed class JsonStringFlexivel : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.GetDecimal().ToString("0.####", CultureInfo.InvariantCulture),
            _ => null,
        };

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}

// int que aceita Number OU String ("200" → 200).
public sealed class JsonIntFlexivel : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32(),
            JsonTokenType.String when int.TryParse(reader.GetString(), NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var n) => n,
            _ => null,
        };

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value.HasValue) writer.WriteNumberValue(value.Value);
        else writer.WriteNullValue();
    }
}

// Cliente da fila de comandos do Clube Seven via Socket.IO (guides/integracao-socket.md).
// Conecta ao servidor com auth { token } (Token do posto), entra na sala machine:<postoId>
// automaticamente e processa os eventos:
//   queue:new  → ack imediato + execução serializada + queue:done
//   queue:sync → backlog de pendentes após (re)conexão
// Config: Fila:Habilitado / Fila:Url / Fila:Token — Url vazia deriva de Backend:WebhookUrl,
// Token vazio cai no Backend:ApiKey (mesmo token do posto usado no header Authorization).
public class QueueSocketService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly IConfiguration _config;
    private readonly ILogger<QueueSocketService> _logger;

    private readonly Channel<FilaDoc> _execucoes = Channel.CreateUnbounded<FilaDoc>(
        new UnboundedChannelOptions { SingleReader = true });

    private volatile SocketIO? _io;
    private volatile bool _conectado;

    // Lido no ctor para o /health já nascer com o estado correto (ExecuteAsync roda depois).
    public bool Habilitado { get; }

    // Estado da conexão com a fila (exposto no /health do painel).
    public bool Conectado => _conectado;

    // Dedup de sessão: comandos já concluídos/não confirmados pelo servidor voltam no
    // queue:sync de cada reconexão — reemitir done sem reexecutar evita aplicar o mesmo
    // comando duas vezes (ex.: preço alterado de novo).
    private readonly object _lockDedup = new();
    private readonly HashSet<string> _executando = new();
    private readonly HashSet<string> _concluidos = new();
    private readonly LinkedList<string> _ordemConcluidos = new();
    private const int MaxConcluidos = 1000;

    // Streams de telemetria ativos (visualizarStream) — chave = código do bico.
    private readonly object _lockStreams = new();
    private readonly Dictionary<string, StreamBico> _streams = new();
    private readonly int _inatividadeStreamMs;

    private sealed class StreamBico
    {
        public required string QueueId { get; init; }
        // Bico exatamente como veio no doc.data — é o valor devolvido ao servidor.
        public required string BicoEcho { get; init; }
        // Código de 2 chars que casa com VisualizacaoBico.Bico (hex do protocolo).
        public required string BicoCode { get; init; }
        public required int IntervaloMs { get; init; }
        public required long IniciadoEm { get; init; }
        public long UltimaMudanca { get; set; }
        public long UltimoEmit { get; set; }
        public decimal? UltimoVolume { get; set; }
    }

    private static readonly JsonSerializerOptions JsonCaseInsensitive = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public QueueSocketService(
        ConcentradorService concentrador,
        IConfiguration config,
        ILogger<QueueSocketService> logger)
    {
        _concentrador = concentrador;
        _config = config;
        _logger = logger;
        Habilitado = bool.TryParse((config["Fila:Habilitado"] ?? "true").Trim(), out var on) && on;
        _inatividadeStreamMs = Math.Max(1_000,
            int.TryParse(config["Fila:StreamInatividadeMs"]?.Trim(), out var ms) ? ms : 60_000);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Habilitado)
        {
            _logger.LogInformation("Fila de comandos desativada (Fila:Habilitado=false)");
            return;
        }

        var (url, token) = ResolverConfig();
        if (url is null || token is null)
        {
            _logger.LogWarning(
                "Fila de comandos sem configuração — defina Fila:Url/Fila:Token (fallbacks: " +
                "Backend:WebhookUrl e Backend:ApiKey)");
            return;
        }

        _logger.LogInformation("Fila de comandos ativa: {Url}", url);
        var consumidor = Task.Run(() => ConsumirFilaAsync(stoppingToken), stoppingToken);
        // Loop único de telemetria: UM C_Visualize por tick atende todos os streams ativos.
        var streamLoop = Task.Run(() => StreamLoopAsync(stoppingToken), stoppingToken);

        var atrasoMs = 3_000;
        while (!stoppingToken.IsCancellationRequested)
        {
            var caiu = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            SocketIO? io = null;
            try
            {
                io = new SocketIO(new Uri(url), new SocketIOOptions
                {
                    Auth = new Dictionary<string, string> { ["token"] = token },
                    Reconnection = false, // reconexão controlada por este loop
                    ConnectionTimeout = TimeSpan.FromSeconds(10),
                });
                _io = io;

                io.OnConnected += (_, _) =>
                {
                    _conectado = true;
                    _logger.LogInformation("Fila: conectado a {Url}", url);
                    // Pede o backlog de pendentes (também cobre a sala machine:<postoId>
                    // reabrindo após queda). Falha de emit aqui não derruba a conexão.
                    _ = Task.Run(async () =>
                    {
                        try { await io.EmitAsync("queue:sync"); }
                        catch (Exception ex) { _logger.LogWarning(ex, "Fila: falha ao pedir queue:sync"); }
                    });
                };
                io.OnDisconnected += (_, motivo) =>
                {
                    _logger.LogWarning("Fila: desconectado ({Motivo})", motivo);
                    caiu.TrySetResult();
                };
                io.OnError += (_, erro) => _logger.LogWarning("Fila: erro do servidor: {Erro}", erro);

                io.On("queue:new", ctx => ReceberDoc(ctx, stoppingToken));
                io.On("queue:sync", ctx => ReceberSync(ctx, stoppingToken));

                await io.ConnectAsync(stoppingToken);
                atrasoMs = 3_000;

                // Aguarda queda (OnDisconnected) com watchdog: se o socket morrer sem
                // evento (timeout de ping etc.), o Connected=false aqui força reconectar.
                while (!stoppingToken.IsCancellationRequested && io.Connected && !caiu.Task.IsCompleted)
                {
                    await Task.WhenAny(caiu.Task, Task.Delay(10_000, stoppingToken));
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConnectionException ex)
            {
                // Token recusado ("Acesso negado" / "Token inválido") ou servidor fora do ar.
                _logger.LogWarning("Fila: conexão recusada por {Url}: {Msg} — nova tentativa em {Ms}ms",
                    url, ex.Message, atrasoMs);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fila: falha na conexão com {Url} — nova tentativa em {Ms}ms",
                    url, atrasoMs);
            }
            finally
            {
                _conectado = false;
                _io = null;
                if (io != null)
                {
                    try { await io.DisconnectAsync(); } catch { /* já caiu */ }
                    io.Dispose();
                }
            }

            // Backoff 3s→15s: fila de comando pede reconexão ágil (uma tentativa por
            // posto é carga desprezível para o servidor).
            if (stoppingToken.IsCancellationRequested) break;
            try { await Task.Delay(atrasoMs, stoppingToken); }
            catch (OperationCanceledException) { break; }
            atrasoMs = Math.Min(atrasoMs * 2, 15_000);
        }

        _execucoes.Writer.TryComplete();
        try { await consumidor; }
        catch (OperationCanceledException) { /* shutdown */ }
        try { await streamLoop; }
        catch (OperationCanceledException) { /* shutdown */ }
    }

    private (string? url, string? token) ResolverConfig()
    {
        var url = _config["Fila:Url"]?.Trim();
        if (string.IsNullOrEmpty(url))
        {
            // O servidor Socket.IO roda na mesma porta da API do backend (guide:
            // ws://<servidor>:8080) — deriva da WebhookUrl quando não informado.
            url = _config["Backend:WebhookUrl"]?.Trim().TrimEnd('/');
        }

        var token = _config["Fila:Token"]?.Trim();
        if (string.IsNullOrEmpty(token))
            token = _config["Backend:ApiKey"]?.Trim();

        return (string.IsNullOrWhiteSpace(url) ? null : url,
                string.IsNullOrWhiteSpace(token) ? null : token);
    }

    private async Task ReceberDoc(IEventContext ctx, CancellationToken ct)
    {
        try
        {
            var doc = ctx.GetValue(typeof(FilaDoc), 0) as FilaDoc;
            if (doc is null || string.IsNullOrEmpty(doc.Id))
            {
                _logger.LogWarning("Fila: queue:new sem _id — ignorado");
                return;
            }
            _logger.LogInformation("Fila: queue:new {Id} action={Action}", doc.Id, doc.Action);
            await AceitarAsync(doc, ct);
        }
        catch (Exception ex)
        {
            // Payload malformado não pode derrubar o handler do socket.
            _logger.LogError(ex, "Fila: falha ao processar queue:new");
        }
    }

    private async Task ReceberSync(IEventContext ctx, CancellationToken ct)
    {
        try
        {
            var docs = ctx.GetValue(typeof(List<FilaDoc>), 0) as List<FilaDoc>;
            if (docs is null || docs.Count == 0)
            {
                _logger.LogInformation("Fila: sync sem pendentes");
                return;
            }

            _logger.LogInformation("Fila: sync com {Total} pendente(s)", docs.Count);
            foreach (var doc in docs)
            {
                if (string.IsNullOrEmpty(doc.Id)) continue;
                await AceitarAsync(doc, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fila: falha ao processar queue:sync");
        }
    }

    // Fluxo do servidor: pending → (queue:ack) → processing → (queue:done) → done.
    private async Task AceitarAsync(FilaDoc doc, CancellationToken ct)
    {
        lock (_lockDedup)
        {
            if (_concluidos.Contains(doc.Id))
            {
                // Já executado nesta sessão mas o done não chegou ao servidor
                // (queda no meio) — reconfirma sem reexecutar.
                _logger.LogInformation("Fila: {Id} já concluído — reenviando done", doc.Id);
                _ = EmitirDoneAsync(doc.Id, 0, sucesso: true, resultado: null, erro: null, ct);
                return;
            }
            if (!_executando.Add(doc.Id)) return; // duplicado em andamento — ignora
        }

        await EmitirAsync("queue:ack", new Dictionary<string, object?> { ["queueId"] = doc.Id }, ct);
        _execucoes.Writer.TryWrite(doc);
    }

    private async Task ConsumirFilaAsync(CancellationToken ct)
    {
        // Leitura SEM cancellation: no shutdown o writer é completado e os docs já
        // aceitos (ack enviados) ainda são executados/tentado done — evita deixar
        // comando pela metade no servidor (processing sem done).
        await foreach (var doc in _execucoes.Reader.ReadAllAsync(CancellationToken.None))
        {
            var sw = Stopwatch.StartNew();
            bool sucesso = false;
            object? resultado = null;
            string? erro = null;
            bool emitirDone = true;
            try
            {
                (sucesso, resultado, emitirDone) = Executar(doc);
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                _logger.LogError(ex, "Fila: erro ao executar {Id} ({Action})", doc.Id, doc.Action);
            }
            sw.Stop();

            lock (_lockDedup)
            {
                _executando.Remove(doc.Id);
                if (emitirDone && _concluidos.Add(doc.Id))
                {
                    _ordemConcluidos.AddLast(doc.Id);
                    if (_ordemConcluidos.Count > MaxConcluidos)
                    {
                        var maisAntigo = _ordemConcluidos.First!.Value;
                        _ordemConcluidos.RemoveFirst();
                        _concluidos.Remove(maisAntigo);
                    }
                }
            }

            if (emitirDone)
                await EmitirDoneAsync(doc.Id, (int)sw.ElapsedMilliseconds, sucesso, resultado, erro, ct);
            // else: visualizarStream — o done sai quando o stream encerrar (StreamLoopAsync),
            // com result = volume final lido (guide §queue:done).
        }
    }

    // Desserializa doc.data com guard — action sem data (ou data não-objeto) devolve null
    // em vez de estourar sobre um JsonElement default.
    private static T? Dados<T>(FilaDoc doc) where T : class =>
        doc.Data.ValueKind == JsonValueKind.Object
            ? doc.Data.Deserialize<T>(JsonCaseInsensitive)
            : null;

    // Executa a action usando a mesma lógica dos endpoints HTTP originais.
    // EmitirDone=false → o comando continua em execução assíncrona (stream); o done
    // é emitido depois, pelo encerramento do stream.
    private (bool sucesso, object? resultado, bool emitirDone) Executar(FilaDoc doc)
    {
        switch (doc.Action?.Trim().ToLowerInvariant())
        {
            case "alterarpreco":
            {
                var d = Dados<AlterarPrecoData>(doc);
                if (string.IsNullOrWhiteSpace(d?.Bico) || string.IsNullOrWhiteSpace(d.Price))
                    throw new ArgumentException("alterarPreco exige bico e price");
                if (!d.Price.All(char.IsDigit))
                    throw new ArgumentException(
                        $"price inválido \"{d.Price}\" — enviar milésimos em dígitos (ex.: 5490 = R$5,490)");

                GarantirConexaoConcentrador();
                var ok = _concentrador.AlterarPreco(d.Bico!, d.Price!);
                if (!ok) _logger.LogWarning("Fila: SetPrice recusado para bico {Bico}", d.Bico);
                return (ok, new { bico = d.Bico, price = d.Price }, true);
            }

            case "visualizarstream":
            {
                var d = Dados<VisualizarStreamData>(doc);
                var bico = d?.Bico?.Trim() ?? "";
                if (bico.Length == 0)
                    throw new ArgumentException("visualizarStream exige bico");

                GarantirConexaoConcentrador();

                // Resolve o código que casa com a resposta do C_Visualize (hex do
                // protocolo, ex. "0D"), aceitando também decimal ("13" → "0D").
                var resp = _concentrador.LerVisualizacaoParsed();
                var code = ResolverBicoCode(bico, resp);

                // Liga o stream contínuo (bico:volume a cada intervalMs); o queue:done
                // só sai quando o stream encerrar (~60s sem atividade / fim do
                // abastecimento), com result = volume final lido.
                IniciarStream(doc.Id, bico, code, Math.Clamp(d?.IntervalMs ?? 200, 200, 5_000));
                return (true, null, false);
            }

            case "checkbridge":
            {
                return (true, new
                {
                    message = "Bridge conectada",
                    conectadoConcentrador = _concentrador.IsConnected,
                }, true);
            }

            default:
                _logger.LogWarning("Fila: action desconhecida '{Action}' ({Id})", doc.Action, doc.Id);
                return (false, null, true);
        }
    }

    // Casa o bico do doc (string livre) com o código do protocolo presente na resposta
    // do C_Visualize: tenta exato ("0D"), depois decimal→hex ("13" → "0D").
    private static string ResolverBicoCode(string bico, VisualizacaoResponse resp)
    {
        var cand = bico.ToUpperInvariant().PadLeft(2, '0');
        if (resp.Bicos.Any(b => b.Bico == cand)) return cand;

        if (cand.Length == 2 && int.TryParse(cand, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n)
            && n is >= 1 and <= 32)
        {
            var hex = n.ToString("X2");
            if (resp.Bicos.Any(b => b.Bico == hex)) return hex;
        }
        return cand;
    }

    // Registra/substitui o stream do bico. Se já havia stream ativo para o mesmo bico,
    // conclui o doc antigo (done com último volume) — guide: novo pedido reinicia o stream.
    private void IniciarStream(string queueId, string bicoEcho, string bicoCode, int intervaloMs)
    {
        var agora = Environment.TickCount64;
        StreamBico? anterior;
        lock (_lockStreams)
        {
            _streams.TryGetValue(bicoCode, out anterior);
            _streams[bicoCode] = new StreamBico
            {
                QueueId = queueId,
                BicoEcho = bicoEcho,
                BicoCode = bicoCode,
                IntervaloMs = intervaloMs,
                IniciadoEm = agora,
                UltimaMudanca = agora,
            };
        }
        _logger.LogInformation(
            "Fila: stream ligado bico {Bico} (código {Code}) a cada {Ms}ms — doc {Id}",
            bicoEcho, bicoCode, intervaloMs, queueId);

        if (anterior != null)
        {
            _logger.LogInformation("Fila: stream anterior do bico {Code} substituído (doc {Id})",
                bicoCode, anterior.QueueId);
            ConcluirDoc(anterior.QueueId);
            _ = EmitirDoneAsync(anterior.QueueId, (int)(agora - anterior.IniciadoEm), true,
                new Dictionary<string, object?> { ["volume"] = anterior.UltimoVolume ?? 0m },
                null, CancellationToken.None);
        }
    }

    // Loop de telemetria (guide §bico:volume): um C_Visualize por tick alimenta todos
    // os streams; volume que muda renova a janela de atividade; sem mudança pelo
    // tempo configurado (default 60s) o stream encerra com done.
    private async Task StreamLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            bool temStreams;
            lock (_lockStreams) temStreams = _streams.Count > 0;
            if (!temStreams)
            {
                await Task.Delay(500, ct);
                continue;
            }

            VisualizacaoResponse? resp = null;
            try { resp = _concentrador.LerVisualizacaoParsed(); }
            catch (Exception ex) { _logger.LogDebug(ex, "Fila: leitura falhou no stream (mantém último volume)"); }

            var agora = Environment.TickCount64;
            var emits = new List<(StreamBico s, decimal vol)>();
            var encerrados = new List<StreamBico>();
            lock (_lockStreams)
            {
                foreach (var s in _streams.Values)
                {
                    var vol = resp?.Bicos.FirstOrDefault(b => b.Bico == s.BicoCode)?.VolumeLitros;
                    if (vol.HasValue && vol.Value != s.UltimoVolume)
                    {
                        s.UltimoVolume = vol.Value;
                        s.UltimaMudanca = agora; // atividade: abastecimento em andamento
                    }

                    if (agora - s.UltimaMudanca >= _inatividadeStreamMs)
                    {
                        encerrados.Add(s);
                        continue;
                    }

                    if (agora - s.UltimoEmit >= s.IntervaloMs)
                    {
                        s.UltimoEmit = agora;
                        emits.Add((s, s.UltimoVolume ?? 0m));
                    }
                }
                foreach (var s in encerrados)
                    _streams.Remove(s.BicoCode);
            }

            var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var (s, vol) in emits)
            {
                await EmitirAsync("bico:volume", new Dictionary<string, object?>
                {
                    ["bico"] = s.BicoEcho,
                    ["volumeLitros"] = vol,
                    ["ts"] = ts,
                }, CancellationToken.None);
            }

            foreach (var s in encerrados)
            {
                _logger.LogInformation(
                    "Fila: stream bico {Bico} encerrado por inatividade — doc {Id} (último volume {Vol})",
                    s.BicoEcho, s.QueueId, s.UltimoVolume ?? 0m);
                ConcluirDoc(s.QueueId);
                await EmitirDoneAsync(s.QueueId, (int)(agora - s.IniciadoEm), true,
                    new Dictionary<string, object?> { ["volume"] = s.UltimoVolume ?? 0m },
                    null, CancellationToken.None);
            }

            await Task.Delay(200, ct);
        }
    }

    // Move um doc para o conjunto de concluídos (dedup de redeliberação pelo sync).
    private void ConcluirDoc(string queueId)
    {
        lock (_lockDedup)
        {
            _executando.Remove(queueId);
            if (_concluidos.Add(queueId))
            {
                _ordemConcluidos.AddLast(queueId);
                if (_ordemConcluidos.Count > MaxConcluidos)
                {
                    var maisAntigo = _ordemConcluidos.First!.Value;
                    _ordemConcluidos.RemoveFirst();
                    _concluidos.Remove(maisAntigo);
                }
            }
        }
    }

    // Best-effort: se o concentrador caiu e o PollingService ainda não reconectou,
    // tenta uma reconexão antes de executar (evita falhar o comando por questões de timing).
    private void GarantirConexaoConcentrador()
    {
        if (!_concentrador.IsConnected && _concentrador.DesejaConectado)
        {
            try { _concentrador.Conectar(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Fila: falha ao reconectar concentrador"); }
        }
    }

    private async Task EmitirDoneAsync(
        string queueId, int responseTimeMs, bool sucesso, object? resultado, string? erro, CancellationToken ct)
    {
        var payload = new Dictionary<string, object?>
        {
            ["queueId"] = queueId,
            ["responseTime"] = responseTimeMs,
            ["success"] = sucesso,
        };
        if (resultado != null) payload["result"] = resultado;
        if (erro != null) payload["erro"] = erro;

        await EmitirAsync("queue:done", payload, ct);
    }

    private async Task EmitirAsync(string evento, object payload, CancellationToken ct)
    {
        var io = _io;
        if (io?.Connected != true)
        {
            _logger.LogWarning("Fila: {Evento} não enviado — socket desconectado", evento);
            return;
        }

        try
        {
            await io.EmitAsync(evento, new[] { payload }, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fila: falha ao emitir {Evento}", evento);
        }
    }
}
