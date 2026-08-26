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

        var intervaloHoras = double.TryParse(_config["Update:IntervaloHoras"], out var h) && h > 0 ? Math.Min(h, 1) : 1;

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

    public record ResultadoAtualizacao(bool Sucesso, string Mensagem, string? VersaoNova = null);

    /// <summary>
    /// Checagem ao vivo (painel "Buscar atualização"): consulta o GitHub agora e atualiza
    /// VersaoMaisRecente — sem esperar o próximo ciclo do loop de background.
    /// </summary>
    public async Task<Version?> VerificarAgoraAsync(CancellationToken ct = default)
    {
        var repo = (_config["Update:Repo"] ?? "caioeliasss/clube-seven-concentrador").Trim();
        var (versao, _) = await ConsultarUltimoRelease(repo, ct);
        if (versao != null)
            VersaoMaisRecente = versao;
        return versao;
    }

    /// <summary>
    /// Disparo manual (painel): consulta o release mais recente e, se houver versão maior,
    /// baixa e aplica ignorando Update:Automatico. Serve quando o auto-update falhou/está desligado.
    /// </summary>
    public async Task<ResultadoAtualizacao> ForcarAtualizacaoAsync(CancellationToken ct = default)
    {
        if (_instaladorLancado)
            return new(true, "Instalação já em andamento — o bridge vai reiniciar em instantes.");

        if (Path.GetFileNameWithoutExtension(Environment.ProcessPath ?? "")
                .Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            return new(false, "Rodando em modo dev (dotnet) — atualização indisponível.");

        var repo = (_config["Update:Repo"] ?? "caioeliasss/clube-seven-concentrador").Trim();

        (Version? versao, string? downloadUrl) release;
        try
        {
            release = await ConsultarUltimoRelease(repo, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateService: falha ao consultar release (manual)");
            return new(false, "Não foi possível consultar o GitHub: " + ex.Message);
        }

        if (release.versao == null)
            return new(false, "Não foi possível obter o último release do GitHub.");

        VersaoMaisRecente = release.versao;

        if (release.versao <= VersaoAtual)
            return new(true, $"Já está na versão mais recente ({VersaoAtual}).");

        if (string.IsNullOrEmpty(release.downloadUrl))
            return new(false, $"Release {release.versao} não tem instalador (ClubeSevenBridge-Setup-*.exe).");

        bool lancado;
        try
        {
            lancado = await BaixarEAplicar(release.downloadUrl, release.versao, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateService: falha na atualização manual");
            return new(false, "Falha ao baixar/aplicar: " + ex.Message);
        }

        // BaixarEAplicar retorna false (sem latchar) se o download for suspeito ou a elevação
        // (UAC) for recusada — nesse caso o operador pode tentar de novo aprovando o prompt.
        if (!lancado)
            return new(false, "Não foi possível iniciar o instalador (download inválido ou UAC recusado). Veja os logs.");

        return new(true, $"Instalando {release.versao} — o bridge vai reiniciar.", release.versao.ToString());
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

        // Se falhar (UAC recusado etc.) não latcha — o próximo ciclo tenta de novo.
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

    /// <summary>
    /// Baixa o instalador e o lança. Retorna true só se o processo do instalador foi realmente
    /// iniciado. Em falha (download suspeito, UAC cancelado, elevação recusada) retorna false
    /// SEM latchar _instaladorLancado — assim a próxima checagem re-tenta em vez de congelar.
    /// </summary>
    private async Task<bool> BaixarEAplicar(string downloadUrl, Version versao, CancellationToken ct)
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
            return false;
        }

        // Lança o instalador silencioso. UseShellExecute=true dispara o UAC (PrivilegesRequired=admin).
        // O instalador fecha este processo via Restart Manager e reinicia o bridge ao final.
        // /LOG: Inno escreve "Setup Log *.txt" no %TEMP% — sem isso uma falha silenciosa é invisível.
        _logger.LogWarning("UpdateService: aplicando atualização {Nova} — o bridge será reiniciado pelo instalador.", versao);
        try
        {
            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = destino,
                Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /NOCANCEL /LOG",
                UseShellExecute = true,
            });
            if (proc == null)
            {
                _logger.LogWarning("UpdateService: Process.Start não retornou processo — instalador não iniciou.");
                return false;
            }

            // Monitora em background: se o instalador sair rápido com código != 0 (falha —
            // ex. arquivo travado, UAC pós-lançamento), reseta o latch para permitir re-tentativa.
            // No caminho feliz o instalador encerra ESTE processo, então a monitoração morre junto.
            _ = Task.Run(async () =>
            {
                try
                {
                    var saiu = await Task.Run(() => proc.WaitForExit(120_000));
                    if (saiu && proc.ExitCode != 0)
                    {
                        _logger.LogError(
                            "UpdateService: instalador falhou com código {Code} — veja \"Setup Log *.txt\" no %TEMP%.",
                            proc.ExitCode);
                        _instaladorLancado = false;
                    }
                }
                catch { /* best-effort: monitoração nunca deve derrubar nada */ }
                finally { proc.Dispose(); }
            });
        }
        catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
        {
            // 1223 = ERROR_CANCELLED: UAC recusado/cancelado. Não latcha — re-tenta depois.
            _logger.LogWarning(ex, "UpdateService: elevação (UAC) recusada — atualização não aplicada. Vai re-tentar no próximo ciclo.");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateService: falha ao lançar o instalador — não latchando para permitir re-tentativa.");
            return false;
        }

        // Só marca como lançado após o Process.Start ter sucesso de fato.
        _instaladorLancado = true;
        return true;
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
