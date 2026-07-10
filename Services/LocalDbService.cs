using LiteDB;
using SevenConcentradorBridge.Models;

namespace SevenConcentradorBridge.Services;

// Banco local embutido (LiteDB). Singleton: uma única LiteDatabase para o processo — a lib
// serializa acessos internamente, então é seguro compartilhar entre os background services.
// O processo --worker nunca chega aqui (retorna em DllWorker.Run antes de montar o host).
public class LocalDbService : IDisposable
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<AbastecimentoRegistroDb> _abastecimentos;
    private readonly ILiteCollection<StatusRegistroDb> _status;
    private readonly ILogger<LocalDbService> _logger;

    public LocalDbService(IConfiguration config, IHostEnvironment env, ILogger<LocalDbService> logger)
    {
        _logger = logger;
        var caminho = AppPaths.CaminhoBanco(env, config);
        _db = new LiteDatabase($"Filename={caminho}");

        _abastecimentos = _db.GetCollection<AbastecimentoRegistroDb>("abastecimentos");
        _abastecimentos.EnsureIndex(x => x.Bico);
        _abastecimentos.EnsureIndex(x => x.CriadoEm);

        _status = _db.GetCollection<StatusRegistroDb>("status");
        _status.EnsureIndex(x => x.CriadoEm);

        _logger.LogInformation("Banco local em {Caminho}", caminho);
    }

    public void InserirAbastecimento(AbastecimentoRegistroDb reg) => _abastecimentos.Insert(reg);

    // Lista abastecimentos mais recentes primeiro (painel / consulta geral).
    public List<AbastecimentoRegistroDb> ListarAbastecimentos(int max) =>
        _abastecimentos.Query().OrderByDescending(x => x.CriadoEm).Limit(max).ToList();

    public int ContarAbastecimentos() => _abastecimentos.Count();

    // Busca abastecimentos por bico e/ou janela de tempo (± toleranciaMin) e/ou litros
    // (± toleranciaLitros). Mesma semântica do ConcentradorService.BuscarAbastecimento, mas
    // contra o banco local em vez da memória do concentrador. Filtros null são ignorados.
    // O bico narrowa na query (indexado); tempo/litros filtram em memória (base pequena).
    public List<AbastecimentoRegistroDb> BuscarAbastecimentos(
        string? bico,
        DateTime? quando,
        int toleranciaMin,
        decimal? litros,
        decimal toleranciaLitros,
        int max)
    {
        var q = _abastecimentos.Query();
        if (!string.IsNullOrWhiteSpace(bico))
        {
            var alvo = bico.Trim().ToUpperInvariant().PadLeft(2, '0');
            q = q.Where(x => x.Bico == alvo);
        }

        IEnumerable<AbastecimentoRegistroDb> itens = q.OrderByDescending(x => x.CriadoEm).ToList();

        if (quando is { } alvoTs)
            itens = itens.Where(x => x.Ts is { } ts
                && Math.Abs((ts - alvoTs).TotalMinutes) <= toleranciaMin);

        if (litros is { } alvoL)
            itens = itens.Where(x => Math.Abs(x.Volume - alvoL) <= toleranciaLitros);

        return itens.Take(max).ToList();
    }

    public List<StatusRegistroDb> ListarStatus(int max) =>
        _status.Query().OrderByDescending(x => x.CriadoEm).Limit(max).ToList();

    public void InserirStatus(string chave, string full) =>
        _status.Insert(new StatusRegistroDb
        {
            StatusChave = chave,
            StatusFull = full,
            CriadoEm = DateTime.Now,
        });

    // Poda por idade: no fluxo pull não há mais "entrega", então todo registro (abastecimento
    // e status) mais velho que a janela de retenção é removido. A janela define até quando o
    // backend consegue buscar uma venda passada — ver Banco:RetencaoMinutos.
    public int PodarAntigos(DateTime limite)
    {
        var removidosAbast = _abastecimentos.DeleteMany(x => x.CriadoEm < limite);
        var removidosStatus = _status.DeleteMany(x => x.CriadoEm < limite);
        return removidosAbast + removidosStatus;
    }

    public void Dispose() => _db.Dispose();
}
