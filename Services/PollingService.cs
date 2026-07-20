using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Services;

namespace SevenConcentradorBridge.Services;

public class PollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<PollingService> _logger;
    private readonly IConfiguration _config;
    private readonly LocalDbService _db;
    private readonly HttpClient _httpClient;
    private readonly NotificationService _notificacao;

    // Chave usada no anti-flood do NotificationService para o problema de conexão.
    private const string ChaveConexao = "conexao";

    public PollingService(
        ConcentradorService concentrador,
        ILogger<PollingService> logger,
        IConfiguration config,
        LocalDbService db,
        IHttpClientFactory httpClientFactory,
        NotificationService notificacao)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _db = db;
        _httpClient = httpClientFactory.CreateClient("Backend");
        _notificacao = notificacao;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Polling:IntervaloMs"] ?? "500");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.Conectar()) break;
                await NotificarProblemaConexao("Falha ao conectar ao concentrador na inicialização.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao concentrador");
                await NotificarProblemaConexao($"Erro ao conectar ao concentrador: {ex.Message}");
            }
            _logger.LogWarning("Tentando reconectar ao concentrador em 5s...");
            await Task.Delay(5000, stoppingToken);
        }

        await NotificarConexaoRecuperada();
        _logger.LogInformation("Polling iniciado com intervalo de {Intervalo}ms", intervalo);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Recupera queda de conexão (cabo/rede) sem precisar reiniciar o processo.
                // Só reconecta se o estado desejado for conectado (não briga com Desconectar manual).
                if (!_concentrador.IsConnected && _concentrador.DesejaConectado)
                {
                    if (!_concentrador.Conectar())
                    {
                        await NotificarProblemaConexao(
                            "Conexão com o concentrador caiu e a reconexão automática falhou. "
                            + "Pode ser queda de rede/cabo ou falha (Access Violation) na DLL nativa.");
                        await Task.Delay(intervalo, stoppingToken);
                        continue;
                    }
                    await NotificarConexaoRecuperada();
                }

                if (_concentrador.IsConnected)
                    await VerificarAbastecimento();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling");
            }

            await Task.Delay(intervalo, stoppingToken);
        }

        _concentrador.Desconectar();
    }

    private async Task NotificarProblemaConexao(string detalhe)
    {
        try
        {
            var tipo = _config["Concentrador:TipoConexao"] ?? "ethernet";
            var destino = tipo == "serial"
                ? $"porta serial {_config["Concentrador:PortaSerial"]}"
                : $"{_config["Concentrador:Ip"]}:{_config["Concentrador:Porta"]}";
            await _notificacao.NotificarProblemaAsync(
                ChaveConexao,
                "Sem conexão com o concentrador",
                $"{detalhe}\n\nConexão: {tipo} ({destino})");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao notificar problema de conexão");
        }
    }

    private async Task NotificarConexaoRecuperada()
    {
        try
        {
            await _notificacao.NotificarRecuperacaoAsync(
                ChaveConexao,
                "Conexão com o concentrador restabelecida",
                "A comunicação com o concentrador foi restabelecida e o polling voltou ao normal.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao notificar recuperação de conexão");
        }
    }

    private async Task VerificarAbastecimento()
    {
        // LerEIncrementar: C_GetSale + C_NextSale atomicamente na thread DLL.
        // Retorna Vazio=true quando não há abastecimento pendente.
        var resp = _concentrador.LerEIncrementar();
        if (resp.Vazio) return;

        // Resíduo de memória: o registro só carrega dia/hora/minuto/mês (sem ano — assumido o ano
        // atual em ConcentradorService.ParseGetSale), então um registro com mais de 1 dia é lixo
        // do buffer e não vale gravar no banco de busca. O ponteiro já avançou (LerEIncrementar).
        // Também não é enviado ao backend (resíduo antigo não vira venda).
        if (resp.Ts is { } ts && ts < DateTime.Now.AddDays(-1))
        {
            _logger.LogWarning(
                "Abastecimento com data {Ts} tem mais de 1 dia — descartado como resíduo (bico={Bico})",
                ts, resp.Bico);
            return;
        }

        _logger.LogInformation(
            "Abastecimento: bico={Bico} total={Total} litros={Vol} data={Ts} raw={Raw}",
            resp.Bico, resp.ValorTotal, resp.Volume, resp.Ts, resp.Raw);

        // Grava sempre no banco local (fonte de busca por bico + horário)...
        _db.InserirAbastecimento(new AbastecimentoRegistroDb
        {
            Bico = resp.Bico,
            Volume = resp.Volume,
            ValorTotal = resp.ValorTotal,
            ValorPorLitro = resp.ValorPorLitro,
            Ts = resp.Ts,
            Raw = resp.Raw,
            CriadoEm = DateTime.Now,
        });

        // ...e também envia ao backend. Só chega aqui abastecimento com menos de 24h (o resíduo
        // antigo já retornou acima), então este é sempre um envio de venda recente.
        await EnviarParaBackend(resp.Raw);
    }

    // POST {Backend:WebhookUrl}/api/concentrador com { comandoRaw, respostaRaw } e
    // Authorization: Bearer <Backend:ApiKey>. Falha de rede não derruba o polling nem
    // desfaz a gravação local — o backend também pode buscar via API depois.
    private async Task EnviarParaBackend(string respostaRaw)
    {
        var apiUrl = (_config["Backend:WebhookUrl"] ?? "").TrimEnd('/');
        var token = _config["Backend:ApiKey"] ?? "";

        if (string.IsNullOrEmpty(apiUrl))
        {
            _logger.LogWarning("Backend:WebhookUrl não configurada — abastecimento não enviado");
            return;
        }

        var url = $"{apiUrl}/api/concentrador";
        var body = JsonSerializer.Serialize(new
        {
            comandoRaw = "C_GetSale",
            respostaRaw,
        });

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Abastecimento enviado ao backend");
            else
                _logger.LogError("Backend retornou {Status} para abastecimento — URL: {Url}", response.StatusCode, url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar abastecimento para {Url}", url);
        }
    }
}
