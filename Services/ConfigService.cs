using System.Text.Json;
using System.Text.Json.Nodes;

namespace SevenConcentradorBridge.Services;

// DTO de configuração editável pelo painel. Espelha as seções do appsettings.json
// que o resto do app lê (Bridge / Auth / Concentrador / Backend / Polling).
public class ConfigDto
{
    public string? BridgePorta { get; set; }

    public string? ConcentradorTipoConexao { get; set; }
    public string? ConcentradorIp { get; set; }
    public string? ConcentradorPorta { get; set; }
    public string? ConcentradorPortaSerial { get; set; }

    public string? BackendWebhookUrl { get; set; }
    public string? BackendApiKey { get; set; }

    public string? PollingIntervaloMs { get; set; }
    public string? PollingStatusIntervaloMs { get; set; }
}

// Lê e grava o MESMO appsettings.json que o host carrega e monitora: o do content root
// (IHostEnvironment.ContentRootPath). Em dev isso é a pasta do projeto; no exe publicado,
// a pasta ao lado do executável. Usar AppContext.BaseDirectory quebrava o dev — gravava na
// cópia do bin enquanto a config era lida do projeto, então o token salvo nunca era visto.
// Usa JsonNode para preservar a estrutura/seções existentes ao reescrever.
// Como CreateBuilder liga reloadOnChange, reescrever o arquivo recarrega _config a quente
// para tudo lido por request; só Bridge:Porta (bind único em app.Urls) exige restart.
public class ConfigService
{
    private readonly ILogger<ConfigService> _logger;
    private readonly string _path;

    public ConfigService(ILogger<ConfigService> logger, IHostEnvironment env)
    {
        _logger = logger;
        _path = Path.Combine(env.ContentRootPath, "appsettings.json");
    }

    public ConfigDto LerConfig()
    {
        var root = LerRoot();
        return new ConfigDto
        {
            BridgePorta = Get(root, "Bridge", "Porta"),
            ConcentradorTipoConexao = Get(root, "Concentrador", "TipoConexao"),
            ConcentradorIp = Get(root, "Concentrador", "Ip"),
            ConcentradorPorta = Get(root, "Concentrador", "Porta"),
            ConcentradorPortaSerial = Get(root, "Concentrador", "PortaSerial"),
            BackendWebhookUrl = Get(root, "Backend", "WebhookUrl"),
            BackendApiKey = Get(root, "Backend", "ApiKey"),
            PollingIntervaloMs = Get(root, "Polling", "IntervaloMs"),
            PollingStatusIntervaloMs = Get(root, "Polling", "StatusIntervaloMs"),
        };
    }

    // Retorna true se a porta do bridge mudou (exige restart para aplicar).
    public bool SalvarConfig(ConfigDto dto)
    {
        var root = LerRoot();
        var portaAntiga = Get(root, "Bridge", "Porta");

        Set(root, "Bridge", "Porta", dto.BridgePorta);
        // Segredo: ignora valor vazio para não apagar a chave existente quando o painel
        // envia o campo mascarado (GET sem auth devolve null).
        Set(root, "Concentrador", "TipoConexao", dto.ConcentradorTipoConexao);
        Set(root, "Concentrador", "Ip", dto.ConcentradorIp);
        Set(root, "Concentrador", "Porta", dto.ConcentradorPorta);
        Set(root, "Concentrador", "PortaSerial", dto.ConcentradorPortaSerial);
        Set(root, "Backend", "WebhookUrl", dto.BackendWebhookUrl);
        Set(root, "Backend", "ApiKey", NaoVazio(dto.BackendApiKey));
        Set(root, "Polling", "IntervaloMs", dto.PollingIntervaloMs);
        Set(root, "Polling", "StatusIntervaloMs", dto.PollingStatusIntervaloMs);

        var json = root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
        _logger.LogInformation("Configuração salva em {Path}", _path);

        return !string.Equals(portaAntiga, dto.BridgePorta, StringComparison.Ordinal);
    }

    // Trata vazio/whitespace como "não informado" (vira null → Set não grava).
    private static string? NaoVazio(string? v) => string.IsNullOrWhiteSpace(v) ? null : v;

    private JsonObject LerRoot()
    {
        if (!File.Exists(_path))
            return new JsonObject();
        var text = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(text))
            return new JsonObject();
        return JsonNode.Parse(text) as JsonObject ?? new JsonObject();
    }

    private static string? Get(JsonObject root, string secao, string chave)
    {
        if (root[secao] is JsonObject obj && obj[chave] is JsonValue v)
            return v.ToString();
        return null;
    }

    // Só grava se o valor foi informado (não nulo); não apaga chaves que o painel não enviou.
    private static void Set(JsonObject root, string secao, string chave, string? valor)
    {
        if (valor is null) return;
        if (root[secao] is not JsonObject obj)
        {
            obj = new JsonObject();
            root[secao] = obj;
        }
        obj[chave] = valor;
    }
}
