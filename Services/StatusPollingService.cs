namespace SevenConcentradorBridge.Services;

// Manual §2.5 — lê status das bombas (LeStatus/C_readState) a cada Polling:StatusIntervaloMs.
// Mantém só o último status em memória; quando muda, grava no histórico local. Fluxo pull:
// não envia nada ao backend — o backend consulta GET /status/historico quando quiser.
public class StatusPollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<StatusPollingService> _logger;
    private readonly IConfiguration _config;
    private readonly LocalDbService _db;

    private string? _ultimoStatus;

    public StatusPollingService(
        ConcentradorService concentrador,
        ILogger<StatusPollingService> logger,
        IConfiguration config,
        LocalDbService db)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _db = db;
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
                if (_concentrador.IsConnected)
                    VerificarStatus();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling de status");
            }

            await Task.Delay(intervalo, stoppingToken);
        }
    }

    private void VerificarStatus()
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

        // Histórico local para consulta. Backend lê GET /status/historico quando precisar.
        try { _db.InserirStatus(chave, status); }
        catch (Exception ex) { _logger.LogError(ex, "Falha ao gravar histórico de status no banco"); }
    }
}
