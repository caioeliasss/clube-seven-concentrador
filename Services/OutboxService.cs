using SevenConcentradorBridge.Models;

namespace SevenConcentradorBridge.Services;

// Reenvia abastecimentos que ficaram Pendentes (backend fora, rede caiu, processo reiniciado)
// e poda registros já resolvidos além da janela de retenção. Roda em paralelo ao PollingService:
// o polling faz a 1ª tentativa; aqui garantimos que nada pendente fique parado.
public class OutboxService : BackgroundService
{
    private readonly LocalDbService _db;
    private readonly ILogger<OutboxService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public OutboxService(
        LocalDbService db,
        ILogger<OutboxService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _logger = logger;
        _config = config;
        _httpClient = httpClientFactory.CreateClient("Backend");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Banco:ReenvioIntervaloMs"] ?? "30000");
        var retencaoMin = int.Parse(_config["Banco:RetencaoMinutos"] ?? "1440");

        _logger.LogInformation(
            "Outbox iniciado: reenvio a cada {Intervalo}ms, retenção {Retencao}min", intervalo, retencaoMin);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ReenviarPendentes(stoppingToken);

                var removidos = _db.PodarAntigos(DateTime.Now.AddMinutes(-retencaoMin));
                if (removidos > 0)
                    _logger.LogInformation("Outbox: {Qtd} registros antigos podados", removidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no ciclo do outbox");
            }

            await Task.Delay(intervalo, stoppingToken);
        }
    }

    private async Task ReenviarPendentes(CancellationToken ct)
    {
        // Lote limitado: evita segurar o backend numa enxurrada se a fila acumulou muito.
        var pendentes = _db.ListarPendentes(50);
        if (pendentes.Count == 0) return;

        _logger.LogInformation("Outbox: reenviando {Qtd} abastecimento(s) pendente(s)", pendentes.Count);

        foreach (var reg in pendentes)
        {
            if (ct.IsCancellationRequested) break;

            reg.Tentativas++;
            var (ok, erro) = await BackendEnvio.EnviarAbastecimentoAsync(_httpClient, _config, reg.Raw, _logger, ct);
            if (ok)
            {
                reg.Status = EntregaStatus.Entregue;
                reg.EntregueEm = DateTime.Now;
            }
            else
            {
                reg.UltimoErro = erro;
                // Backend indisponível: para o lote e tenta de novo no próximo ciclo (mantém FIFO).
                _db.AtualizarAbastecimento(reg);
                break;
            }
            _db.AtualizarAbastecimento(reg);
        }
    }
}
