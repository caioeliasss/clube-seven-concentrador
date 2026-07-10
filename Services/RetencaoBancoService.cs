namespace SevenConcentradorBridge.Services;

// Manutenção do banco local: poda registros (abastecimentos e status) mais velhos que a janela
// de retenção. No fluxo pull não há reenvio — o backend busca via API — então este serviço só
// controla o crescimento do arquivo. A janela (Banco:RetencaoMinutos) também define até quando
// o backend consegue buscar uma venda passada.
public class RetencaoBancoService : BackgroundService
{
    private readonly LocalDbService _db;
    private readonly ILogger<RetencaoBancoService> _logger;
    private readonly IConfiguration _config;

    public RetencaoBancoService(
        LocalDbService db,
        ILogger<RetencaoBancoService> logger,
        IConfiguration config)
    {
        _db = db;
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Banco:PodaIntervaloMs"] ?? "300000"); // 5 min
        var retencaoMin = int.Parse(_config["Banco:RetencaoMinutos"] ?? "10080"); // 7 dias

        _logger.LogInformation(
            "Retenção do banco: poda a cada {Intervalo}ms, janela {Retencao}min", intervalo, retencaoMin);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var removidos = _db.PodarAntigos(DateTime.Now.AddMinutes(-retencaoMin));
                if (removidos > 0)
                    _logger.LogInformation("Retenção: {Qtd} registros antigos podados", removidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na poda do banco");
            }

            await Task.Delay(intervalo, stoppingToken);
        }
    }
}
