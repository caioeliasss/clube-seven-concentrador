using System.Net.Http.Headers;

namespace SevenConcentradorBridge.Services;

// Verifica se o backend configurado em Backend:WebhookUrl reconhece a
// Backend:ApiKey atual: POST {WebhookUrl}/api/concentrador/key/check com
// Authorization: Bearer <Backend:ApiKey>. Puramente informativo (indicador
// "Webhook / Backend" no painel) — quem decide acesso ao bridge é o
// ApiKeyService (local), não este serviço.
public class BackendStatusService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<BackendStatusService> _logger;

    public BackendStatusService(
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        ILogger<BackendStatusService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    public Task<bool> VerificarAsync(CancellationToken ct) => VerificarAsync(null, null, ct);

    // Aceita override de url/key para testar valores ainda não salvos (painel testa
    // antes de persistir). Sem override, cai nos valores atuais do appsettings.json.
    public async Task<bool> VerificarAsync(string? webhookUrlOverride, string? apiKeyOverride, CancellationToken ct)
    {
        var apiUrl = (webhookUrlOverride ?? _config["Backend:WebhookUrl"] ?? "").TrimEnd('/');
        var key = apiKeyOverride ?? _config["Backend:ApiKey"] ?? "";
        if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(key))
            return false;

        var url = $"{apiUrl}/api/concentrador/key/check";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

        try
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.SendAsync(request, ct);

            // O backend sinaliza pelo status HTTP: 2xx = token reconhecido, 401 = recusado.
            // Não exigimos formato de corpo específico (cada backend devolve o que quiser).
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Backend /key/check retornou {Status}", response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao verificar backend {Url}", url);
            return false;
        }
    }
}
