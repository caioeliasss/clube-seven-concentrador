using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Services;

namespace SevenConcentradorBridge.Services;

public class PollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<PollingService> _logger;
    private readonly IConfiguration _config;
    private readonly LocalDbService _db;

    public PollingService(
        ConcentradorService concentrador,
        ILogger<PollingService> logger,
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
        var intervalo = int.Parse(_config["Polling:IntervaloMs"] ?? "500");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.Conectar()) break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao concentrador");
            }
            _logger.LogWarning("Tentando reconectar ao concentrador em 5s...");
            await Task.Delay(5000, stoppingToken);
        }

        _logger.LogInformation("Polling iniciado com intervalo de {Intervalo}ms", intervalo);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Recupera queda de conexão (cabo/rede) sem precisar reiniciar o processo.
                // Só reconecta se o estado desejado for conectado (não briga com Desconectar manual).
                if (!_concentrador.IsConnected && _concentrador.DesejaConectado)
                {
                    if (!_concentrador.Conectar())
                    {
                        await Task.Delay(intervalo, stoppingToken);
                        continue;
                    }
                }

                if (_concentrador.IsConnected)
                    VerificarAbastecimento();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling");
            }

            await Task.Delay(intervalo, stoppingToken);
        }

        _concentrador.Desconectar();
    }

    private void VerificarAbastecimento()
    {
        // LerEIncrementar: C_GetSale + C_NextSale atomicamente na thread DLL.
        // Retorna Vazio=true quando não há abastecimento pendente.
        var resp = _concentrador.LerEIncrementar();
        if (resp.Vazio) return;

        // Resíduo de memória: o registro só carrega dia/hora/minuto/mês (sem ano — assumido o ano
        // atual em ConcentradorService.ParseGetSale), então um registro com mais de 1 dia é lixo
        // do buffer e não vale gravar no banco de busca. O ponteiro já avançou (LerEIncrementar).
        if (resp.Ts is { } ts && ts < DateTime.Now.AddDays(-1))
        {
            _logger.LogWarning(
                "Abastecimento com data {Ts} tem mais de 1 dia — descartado como resíduo (bico={Bico})",
                ts, resp.Bico);
            return;
        }

        _logger.LogInformation(
            "Abastecimento: bico={Bico} total={Total} litros={Vol} data={Ts} raw={Raw}",
            resp.Bico, resp.ValorTotal, resp.Volume, resp.Ts, resp.Raw);

        // Fluxo pull: só grava no banco. O backend busca via API (bico + horário) quando precisar.
        _db.InserirAbastecimento(new AbastecimentoRegistroDb
        {
            Bico = resp.Bico,
            Volume = resp.Volume,
            ValorTotal = resp.ValorTotal,
            ValorPorLitro = resp.ValorPorLitro,
            Ts = resp.Ts,
            Raw = resp.Raw,
            CriadoEm = DateTime.Now,
        });
    }
}
