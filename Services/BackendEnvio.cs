using System.Text;
using System.Text.Json;

namespace SevenConcentradorBridge.Services;

// Envio de abastecimento ao backend, centralizado para PollingService (primeira tentativa)
// e OutboxService (reenvio dos pendentes) compartilharem exatamente a mesma requisição.
// Retorna (ok, erro): ok=true só em 2xx; erro traz motivo p/ gravar no outbox.
public static class BackendEnvio
{
    public static async Task<(bool ok, string? erro)> EnviarAbastecimentoAsync(
        HttpClient http, IConfiguration config, string respostaRaw, ILogger logger, CancellationToken ct)
    {
        // Fonte preferida: appsettings Backend:* (editável pelo painel, hot-reload).
        // Fallback: API_URL/TOKEN do .env (compatibilidade).
        var apiUrl = (config["Backend:WebhookUrl"] ?? config["API_URL"] ?? "").TrimEnd('/');
        var token = config["Backend:ApiKey"] ?? config["TOKEN"] ?? "";

        if (string.IsNullOrEmpty(apiUrl))
        {
            logger.LogWarning("Backend:WebhookUrl/API_URL não configurada — abastecimento não enviado");
            return (false, "backend não configurado");
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
            var response = await http.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Abastecimento enviado ao backend");
                return (true, null);
            }
            logger.LogError("Backend retornou {Status} para abastecimento — URL: {Url}", response.StatusCode, url);
            return (false, $"HTTP {(int)response.StatusCode}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Falha ao enviar abastecimento para {Url}", url);
            return (false, ex.Message);
        }
    }
}
