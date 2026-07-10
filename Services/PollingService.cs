using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Services;

namespace SevenConcentradorBridge.Services;

public class PollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<PollingService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly LocalDbService _db;

    public PollingService(
        ConcentradorService concentrador,
        ILogger<PollingService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        LocalDbService db)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _httpClient = httpClientFactory.CreateClient("Backend");
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
                    await VerificarAbastecimento(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling");
            }

            await Task.Delay(intervalo, stoppingToken);
        }

        _concentrador.Desconectar();
    }

    private async Task VerificarAbastecimento(CancellationToken ct)
    {
        // LerEIncrementar: C_GetSale + C_NextSale atomicamente na thread DLL.
        // Retorna Vazio=true quando não há abastecimento pendente.
        var resp = _concentrador.LerEIncrementar();
        if (resp.Vazio) return;

        _logger.LogInformation(
            "Abastecimento: bico={Bico} total={Total} litros={Vol} data={Ts} raw={Raw}",
            resp.Bico, resp.ValorTotal, resp.Volume, resp.Ts, resp.Raw);

        // Grava no outbox ANTES de enviar. Como o ponteiro do concentrador já avançou
        // (C_NextSale), esta é a única cópia da venda — se o processo cair ou o backend
        // estiver fora, o OutboxService reenvia a partir daqui.
        var reg = new AbastecimentoRegistroDb
        {
            Bico = resp.Bico,
            Volume = resp.Volume,
            ValorTotal = resp.ValorTotal,
            ValorPorLitro = resp.ValorPorLitro,
            Ts = resp.Ts,
            Raw = resp.Raw,
            CriadoEm = DateTime.Now,
            Status = EntregaStatus.Pendente,
        };

        // Não envia abastecimentos antigos (> 1 dia). O registro só carrega dia/hora/minuto/mês
        // (sem ano — assumido o ano atual em ConcentradorService.ParseGetSale), então um registro
        // com mais de 24h é resíduo de memória e não deve gerar webhook no backend.
        if (resp.Ts is { } ts && ts < DateTime.Now.AddDays(-1))
        {
            _logger.LogWarning(
                "Abastecimento com data {Ts} tem mais de 1 dia — não enviado ao backend (bico={Bico})",
                ts, resp.Bico);
            reg.Status = EntregaStatus.Ignorado;
            reg.UltimoErro = "descartado: mais de 1 dia";
        }

        _db.InserirAbastecimento(reg);
        if (reg.Status != EntregaStatus.Pendente) return;

        reg.Tentativas++;
        var (ok, erro) = await BackendEnvio.EnviarAbastecimentoAsync(_httpClient, _config, resp.Raw, _logger, ct);
        if (ok)
        {
            reg.Status = EntregaStatus.Entregue;
            reg.EntregueEm = DateTime.Now;
        }
        else
        {
            reg.UltimoErro = erro;
        }
        _db.AtualizarAbastecimento(reg);
    }
}
