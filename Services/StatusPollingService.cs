using System.Text;
using System.Text.Json;

namespace SevenConcentradorBridge.Services;

// Manual §2.5 — lê status das bombas (LeStatus/C_readState) a cada Polling:StatusIntervaloMs.
// Mantém só o último status em memória; quando muda, POST para {API_URL}/api/concentrador/status.
public class StatusPollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<StatusPollingService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    private string? _ultimoStatus;

    public StatusPollingService(
        ConcentradorService concentrador,
        ILogger<StatusPollingService> logger,
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

        // Só os primeiros 33 chars são o status real; tail (versão/checksum) muda a cada leitura.
        string chave = status.Length >= 33 ? status[..33] : status;
        if (chave == _ultimoStatus) return;

        _ultimoStatus = chave;
        _logger.LogInformation("Status mudou: {Status}", status);
        await EnviarParaBackend(status, ct);
    }

    private async Task EnviarParaBackend(string statusString, CancellationToken ct)
    {
        var apiUrl = (_config["API_URL"] ?? "").TrimEnd('/');
        var token = _config["TOKEN"] ?? "";

        if (string.IsNullOrEmpty(apiUrl))
        {
            _logger.LogWarning("API_URL não configurada — status não enviado");
            return;
        }

        var url = $"{apiUrl}/api/concentrador/status";
        var body = JsonSerializer.Serialize(new { statusString });

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await _httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Status enviado ao backend");
            else
                _logger.LogError("Backend retornou {Status} para status — URL: {Url}", response.StatusCode, url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar status para {Url}", url);
        }
    }
}
