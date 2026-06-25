using System.Security.Cryptography;
using System.Text;

namespace SevenConcentradorBridge.Services;

// Valida a key recebida (Authorization: Bearer) comparando localmente com
// Backend:ApiKey no appsettings.json. É a mesma key usada pelo bridge para se
// autenticar no backend ao enviar webhooks (PollingService/StatusPollingService) —
// um único segredo compartilhado, sem round-trip de rede pra validar o painel.
public class ApiKeyService
{
    private readonly IConfiguration _config;

    public ApiKeyService(IConfiguration config)
    {
        _config = config;
    }

    public bool ValidarKey(string? key)
    {
        var esperada = _config["Backend:ApiKey"];
        if (string.IsNullOrEmpty(esperada) || string.IsNullOrEmpty(key))
            return false;

        var a = Encoding.UTF8.GetBytes(key);
        var b = Encoding.UTF8.GetBytes(esperada);
        return a.Length == b.Length && CryptographicOperations.FixedTimeEquals(a, b);
    }
}
