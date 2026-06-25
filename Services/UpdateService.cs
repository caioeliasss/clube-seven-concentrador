using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace SevenConcentradorBridge.Services;

/// <summary>
/// Auto-update: verifica periodicamente o GitHub Releases do repositório configurado,
/// e quando há uma versão maior que a atual baixa o instalador (ClubeSevenBridge-Setup-*.exe)
/// e o roda em modo silencioso. O próprio instalador (setup.iss) fecha este processo via
/// Restart Manager (AppMutex/CloseApplications), substitui os arquivos e reinicia o bridge.
///
/// Repositório público → API e download do asset funcionam sem token.
/// Config: Update:Repo, Update:IntervaloHoras, Update:Automatico.
/// </summary>
public class UpdateService : BackgroundService
{
    private readonly ILogger<UpdateService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    /// <summary>Versão em execução (do assembly), normalizada para X.Y.Z.</summary>
    public Version VersaoAtual { get; }
    /// <summary>Última versão vista no GitHub (null até a primeira checagem bem-sucedida).</summary>
    public Version? VersaoMaisRecente { get; private set; }
    public bool AtualizacaoDisponivel =>
        VersaoMaisRecente != null && VersaoMaisRecente > VersaoAtual;

    private bool _instaladorLancado;

    public UpdateService(
        ILogger<UpdateService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _http = httpClientFactory.CreateClient();
        _http.Timeout = TimeSpan.FromMinutes(5); // download do setup pode ser grande
        // GitHub exige User-Agent.
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("ClubeSevenBridge-Updater");

        var v = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0);
        VersaoAtual = Normalizar(v);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Só faz sentido no exe publicado (single-file). Em dev (rodando via dotnet) o caminho
        // do processo é o dotnet e não há o que atualizar — pula.
        if (Path.GetFileNameWithoutExtension(Environment.ProcessPath ?? "")
                .Equals("dotnet", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("UpdateService: rodando via dotnet (dev), auto-update desativado.");
            return;
        }

        var intervaloHoras = double.TryParse(_config["Update:IntervaloHoras"], out var h) && h > 0 ? h : 6;

        // Delay inicial para não competir com a subida do host/concentrador.
        try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
        catch (OperationCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VerificarEAtualizar(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateService: erro na checagem de atualização");
            }

            try { await Task.Delay(TimeSpan.FromHours(intervaloHoras), stoppingToken); }
            catch (OperationCanceledException) { return; }
        }
    }

    private async Task VerificarEAtualizar(CancellationToken ct)
    {
        if (_instaladorLancado) return; // já disparou o upgrade; aguardando o instalador fechar.

        var repo = (_config["Update:Repo"] ?? "caioeliasss/clube-seven-concentrador").Trim();
        var automatico = !bool.TryParse(_config["Update:Automatico"], out var a) || a; // default true

        var (versao, downloadUrl) = await ConsultarUltimoRelease(repo, ct);
        if (versao == null)
            return;

        VersaoMaisRecente = versao;

        if (versao <= VersaoAtual)
        {
            _logger.LogInformation("UpdateService: já na versão mais recente ({Atual}).", VersaoAtual);
            return;
        }

        _logger.LogInformation("UpdateService: atualização disponível {Atual} -> {Nova}.",
            VersaoAtual, versao);

        if (!automatico)
        {
            _logger.LogInformation("UpdateService: Update:Automatico=false — não instalando automaticamente.");
            return;
        }

        if (string.IsNullOrEmpty(downloadUrl))
        {
            _logger.LogWarning("UpdateService: release {Nova} sem asset ClubeSevenBridge-Setup-*.exe.", versao);
            return;
        }

        await BaixarEAplicar(downloadUrl, versao, ct);
    }

    /// <summary>Consulta releases/latest e retorna (versão, url do asset do instalador).</summary>
    private async Task<(Version? versao, string? downloadUrl)> ConsultarUltimoRelease(string repo, CancellationToken ct)
    {
        var url = $"https://api.github.com/repos/{repo}/releases/latest";
        using var resp = await _http.GetAsync(url, ct);
        if (!resp.IsSuccessStatusCode)
        {
            _logger.LogWarning("UpdateService: GitHub retornou {Status} para {Url}.", resp.StatusCode, url);
            return (null, null);
        }

        await using var stream = await resp.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        var root = doc.RootElement;

        if (!root.TryGetProperty("tag_name", out var tagEl)) return (null, null);
        var versao = ParseTag(tagEl.GetString());
        if (versao == null) return (null, null);

        string? downloadUrl = null;
        if (root.TryGetProperty("assets", out var assets) && assets.ValueKind == JsonValueKind.Array)
        {
            foreach (var asset in assets.EnumerateArray())
            {
                var nome = asset.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
                if (nome.StartsWith("ClubeSevenBridge-Setup", StringComparison.OrdinalIgnoreCase)
                    && nome.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                    && asset.TryGetProperty("browser_download_url", out var dl))
                {
                    downloadUrl = dl.GetString();
                    break;
                }
            }
        }

        return (versao, downloadUrl);
    }

    private async Task BaixarEAplicar(string downloadUrl, Version versao, CancellationToken ct)
    {
        var destino = Path.Combine(Path.GetTempPath(), $"ClubeSevenBridge-Setup-{versao}.exe");

        _logger.LogInformation("UpdateService: baixando {Url} para {Destino}.", downloadUrl, destino);
        using (var resp = await _http.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, ct))
        {
            resp.EnsureSuccessStatusCode();
            await using var fs = File.Create(destino);
            await resp.Content.CopyToAsync(fs, ct);
        }

        var info = new FileInfo(destino);
        if (!info.Exists || info.Length < 1024 * 1024) // setup self-contained tem dezenas de MB
        {
            _logger.LogWarning("UpdateService: download suspeito ({Bytes} bytes) — abortando.", info.Exists ? info.Length : 0);
            try { File.Delete(destino); } catch { }
            return;
        }

        // Lança o instalador silencioso. UseShellExecute=true dispara o UAC (PrivilegesRequired=admin).
        // O instalador fecha este processo via Restart Manager e reinicia o bridge ao final.
        _logger.LogWarning("UpdateService: aplicando atualização {Nova} — o bridge será reiniciado pelo instalador.", versao);
        _instaladorLancado = true;
        Process.Start(new ProcessStartInfo
        {
            FileName = destino,
            Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /NOCANCEL",
            UseShellExecute = true,
        });
    }

    /// <summary>Tag "v0.7.3" / "0.7.3" → Version normalizada X.Y.Z.</summary>
    private static Version? ParseTag(string? tag)
    {
        if (string.IsNullOrWhiteSpace(tag)) return null;
        tag = tag.Trim().TrimStart('v', 'V');
        return Version.TryParse(tag, out var v) ? Normalizar(v) : null;
    }

    /// <summary>Zera componentes ausentes (-1) e descarta revision para comparar só X.Y.Z.</summary>
    private static Version Normalizar(Version v) =>
        new(v.Major, Math.Max(v.Minor, 0), Math.Max(v.Build, 0));
}
