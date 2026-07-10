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
        _abastecimentos.EnsureIndex(x => x.Status);
        _abastecimentos.EnsureIndex(x => x.CriadoEm);

        _status = _db.GetCollection<StatusRegistroDb>("status");
        _status.EnsureIndex(x => x.CriadoEm);

        _logger.LogInformation("Banco local em {Caminho}", caminho);
    }

    public void InserirAbastecimento(AbastecimentoRegistroDb reg) => _abastecimentos.Insert(reg);

    public void AtualizarAbastecimento(AbastecimentoRegistroDb reg) => _abastecimentos.Update(reg);

    // Pendentes mais antigos primeiro (ordem de chegada) — mantém a fila FIFO no reenvio.
    public List<AbastecimentoRegistroDb> ListarPendentes(int max) =>
        _abastecimentos.Query()
            .Where(x => x.Status == EntregaStatus.Pendente)
            .OrderBy(x => x.CriadoEm)
            .Limit(max)
            .ToList();

    // Lista abastecimentos mais recentes primeiro (para exibir no painel), opcionalmente
    // filtrando por status de entrega. null = todos.
    public List<AbastecimentoRegistroDb> ListarAbastecimentos(EntregaStatus? status, int max)
    {
        var q = _abastecimentos.Query();
        if (status is { } s) q = q.Where(x => x.Status == s);
        return q.OrderByDescending(x => x.CriadoEm).Limit(max).ToList();
    }

    // Quantos abastecimentos existem por status — usado no resumo do painel.
    public (int pendentes, int entregues, int ignorados) ContarAbastecimentos() => (
        _abastecimentos.Count(x => x.Status == EntregaStatus.Pendente),
        _abastecimentos.Count(x => x.Status == EntregaStatus.Entregue),
        _abastecimentos.Count(x => x.Status == EntregaStatus.Ignorado));

    public List<StatusRegistroDb> ListarStatus(int max) =>
        _status.Query().OrderByDescending(x => x.CriadoEm).Limit(max).ToList();

    public void InserirStatus(string chave, string full) =>
        _status.Insert(new StatusRegistroDb
        {
            StatusChave = chave,
            StatusFull = full,
            CriadoEm = DateTime.Now,
        });

    // Poda o que já saiu da fila (Entregue/Ignorado) e o histórico de status mais velhos que
    // o limite. Pendentes de abastecimento são preservados sempre — a durabilidade depende disso.
    public int PodarAntigos(DateTime limite)
    {
        var removidosAbast = _abastecimentos.DeleteMany(x =>
            x.Status != EntregaStatus.Pendente && x.CriadoEm < limite);
        var removidosStatus = _status.DeleteMany(x => x.CriadoEm < limite);
        return removidosAbast + removidosStatus;
    }

    public void Dispose() => _db.Dispose();
}
