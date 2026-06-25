using System.Text;
using System.Text.Json;
using SevenConcentradorBridge.Services;

namespace SevenConcentradorBridge.Services;

public class PollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<PollingService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public PollingService(
        ConcentradorService concentrador,
        ILogger<PollingService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _httpClient = httpClientFactory.CreateClient("Backend");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Polling:IntervaloMs"] ?? "500");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.Conectar()) break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao concentrador");
            }
            _logger.LogWarning("Tentando reconectar ao concentrador em 5s...");
            await Task.Delay(5000, stoppingToken);
        }

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
                        await Task.Delay(intervalo, stoppingToken);
                        continue;
                    }
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

    private async Task VerificarAbastecimento()
    {
        // LerEIncrementar: C_GetSale + C_NextSale atomicamente na thread DLL.
        // Retorna Vazio=true quando não há abastecimento pendente.
        var resp = _concentrador.LerEIncrementar();
        if (resp.Vazio) return;

        _logger.LogInformation(
            "Abastecimento: bico={Bico} total={Total} litros={Vol} raw={Raw}",
            resp.Bico, resp.ValorTotal, resp.Volume, resp.Raw);

        await EnviarParaBackend(resp.Raw);
    }

    private async Task EnviarParaBackend(string respostaRaw)
    {
        // Fonte preferida: appsettings Backend:* (editável pelo painel, hot-reload).
        // Fallback: API_URL/TOKEN do .env (compatibilidade).
        var apiUrl = (_config["Backend:WebhookUrl"] ?? _config["API_URL"] ?? "").TrimEnd('/');
        var token = _config["Backend:ApiKey"] ?? _config["TOKEN"] ?? "";

        if (string.IsNullOrEmpty(apiUrl))
        {
            _logger.LogWarning("Backend:WebhookUrl/API_URL não configurada — abastecimento não enviado");
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
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
