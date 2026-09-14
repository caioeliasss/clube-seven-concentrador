using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SevenConcentradorBridge.Services;

// Manual §2.5 — lê status das bombas (LeStatus/C_readState) a cada Polling:StatusIntervaloMs.
// Toda mudança de status é gravada no histórico local e POSTada para
// {Backend:WebhookUrl}/api/concentrador/status (com reenvio até o backend confirmar).
// Backend também pode consultar GET /status/historico.
public class StatusPollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<StatusPollingService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly LocalDbService _db;

    // Dedup em duas camadas: registro local (histórico) e envio ao backend.
    // _ultimoEnviadoOk só avança quando o backend confirma (2xx) — falha de rede
    // não descarta a mudança; o próximo ciclo relê e reenvia.
    private string? _ultimoRegistrado;
    private string? _ultimoEnviadoOk;
    private DateTime _proximaTentativaEnvio = DateTime.MinValue;

    public StatusPollingService(
        ConcentradorService concentrador,
        ILogger<StatusPollingService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        LocalDbService db)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _httpClient = httpClientFactory.CreateClient("Backend");
        _db = db;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Polling:StatusIntervaloMs"] ?? "200");

        // Espera o PollingService (ou ele mesmo) estabelecer a conexão.
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.Conectar()) break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Status: erro ao conectar ao concentrador");
            }
            await Task.Delay(5000, stoppingToken);
        }

        _logger.LogInformation("Status polling iniciado com intervalo de {Intervalo}ms", intervalo);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.IsConnected)
                    await VerificarStatus(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling de status");
            }

            await Task.Delay(intervalo, stoppingToken);
        }
    }

    private async Task VerificarStatus(CancellationToken ct)
    {
        string status;
        try
        {
            status = _concentrador.LerStatus();
        }
        catch (InvalidOperationException)
        {
            // Desconectado — PollingService cuida da reconexão.
            return;
        }

        if (string.IsNullOrEmpty(status)) return;

        // "SEM RESPOSTA"/"FALHA" indica queda (LerStatus já derrubou a conexão) — não é status
        // de bomba: não grava no histórico nem vira mudança, senão envenena o estado e engole
        // o próximo envio real.
        if (ConcentradorService.RespostaIndicaQueda(status)) return;

        string chave = ChaveStatus(status);
        if (chave == _ultimoRegistrado && chave == _ultimoEnviadoOk) return;

        // Histórico local para auditoria/consulta. Status é snapshot — registramos cada mudança.
        if (chave != _ultimoRegistrado)
        {
            _ultimoRegistrado = chave;
            _logger.LogInformation("Status mudou: {Status}", status);

            try { _db.InserirStatus(chave, status); }
            catch (Exception ex) { _logger.LogError(ex, "Falha ao gravar histórico de status no banco"); }
        }

        // Só pula o envio quando ESTE status já foi confirmado pelo backend.
        if (chave == _ultimoEnviadoOk) return;
        if (DateTime.UtcNow < _proximaTentativaEnvio) return;

        if (await EnviarParaBackend(status, ct))
            _ultimoEnviadoOk = chave;
        else
            _proximaTentativaEnvio = DateTime.UtcNow.AddSeconds(2); // backoff p/ não martelar backend caído
    }

    // Resposta &S (§3.4.1): "(SXXXXXXXX...FFDDCVVVVMMMMPTT)" — 'S' + status dos endereços
    // (32 no CBC padrão, 36 no Horustech) + cauda (fixo/dip/versão/tensão) que varia a cada
    // leitura. A chave cobre todo o bloco de status; o framing com parênteses é descartado —
    // truncar os chars crus em 33 cortava o último endereço quando a DLL devolve "(S...)".
    private static string ChaveStatus(string status)
    {
        string p = status.Trim();
        if (p.StartsWith("(")) p = p[1..];
        if (p.EndsWith(")")) p = p[..^1];
        return p.Length <= 37 ? p : p[..37];
    }

    // POST {Backend:WebhookUrl}/api/concentrador/status com { statusString } e
    // Authorization: Bearer <Backend:ApiKey>. Retorna true quando o backend confirmou
    // (2xx) — quem chama só marca o status como enviado nesse caso, garantindo reenvio
    // na próxima leitura após falha. Falha de rede não derruba o polling nem desfaz a
    // gravação local — o backend também pode buscar via /status/historico.
    private async Task<bool> EnviarParaBackend(string statusString, CancellationToken ct)
    {
        var apiUrl = (_config["Backend:WebhookUrl"] ?? _config["API_URL"] ?? "").TrimEnd('/');
        var token = _config["Backend:ApiKey"] ?? _config["TOKEN"] ?? "";

        if (string.IsNullOrEmpty(apiUrl))
        {
            // Sem webhook configurado não há pra onde enviar — trata como ack para não
            // reprocessar a mesma mudança a cada ciclo (comportamento igual ao antigo).
            _logger.LogWarning("Backend:WebhookUrl/API_URL não configurada — status não enviado");
            return true;
        }

        var url = $"{apiUrl}/api/concentrador/status";
        var body = JsonSerializer.Serialize(new { statusString });

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await _httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Status enviado ao backend");
                return true;
            }
            _logger.LogError("Backend retornou {Status} para status — URL: {Url}", response.StatusCode, url);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutdown — não loga como erro.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar status para {Url}", url);
        }
        return false;
    }
}
