using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SevenConcentradorBridge.Services;

// Valida a API Key contra o backend (api.clubeseven.com), não mais contra o
// appsettings local. POST {Backend:WebhookUrl}/api/concentrador/api/check com
// Authorization: Bearer <key>; o backend responde { "success": true|false }.
//
// Cache curto em memória para não bater no backend a cada request protegido
// (o painel e o webhook chamam endpoints com frequência). TTL configurável em
// Auth:CacheKeyMs (default 30s).
public class BackendAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<BackendAuthService> _logger;
    private readonly ConcurrentDictionary<string, (bool valido, DateTime expira)> _cache = new();

    public BackendAuthService(
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        ILogger<BackendAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    public async Task<bool> ValidarKeyAsync(string? key, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        if (_cache.TryGetValue(key, out var hit) && hit.expira > DateTime.UtcNow)
            return hit.valido;

        var valido = await ConsultarBackendAsync(key, ct);

        var ttl = int.TryParse(_config["Auth:CacheKeyMs"], out var ms) ? ms : 30_000;
        _cache[key] = (valido, DateTime.UtcNow.AddMilliseconds(ttl));
        return valido;
    }

    private async Task<bool> ConsultarBackendAsync(string key, CancellationToken ct)
    {
        var apiUrl = (_config["Backend:WebhookUrl"] ?? _config["API_URL"] ?? "").TrimEnd('/');
        if (string.IsNullOrEmpty(apiUrl))
        {
            _logger.LogWarning("Backend:WebhookUrl não configurada — não dá para validar a key");
            return false;
        }

        var url = $"{apiUrl}/api/concentrador/key/check";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

        try
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Backend /key/check retornou {Status}", response.StatusCode);
                return false;
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("success", out var s) && s.GetBoolean();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao validar key no backend {Url}", url);
            return false;
        }
    }
}
