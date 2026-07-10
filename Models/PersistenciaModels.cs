namespace SevenConcentradorBridge.Models;

// Estado de entrega de um abastecimento ao backend.
// Pendente  = lido do concentrador, ainda não confirmado pelo backend (2xx). NUNCA é podado por tempo.
// Entregue  = backend respondeu 2xx. Pode ser podado após a janela de retenção.
// Ignorado  = registro descartado de propósito (ex.: resíduo com mais de 1 dia). Também podável.
public enum EntregaStatus
{
    Pendente = 0,
    Entregue = 1,
    Ignorado = 2,
}

// Outbox durável de abastecimentos. Motivo de existir: o polling faz C_GetSale + C_NextSale
// atômico — quando lemos, o ponteiro do concentrador já avançou e o registro saiu do buffer.
// Se o POST ao backend falhar sem isto, a venda se perde. Aqui gravamos ANTES de enviar e
// reenviamos os Pendentes até confirmar.
public class AbastecimentoRegistroDb
{
    public int Id { get; set; }
    public string Bico { get; set; } = "";
    public decimal Volume { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorPorLitro { get; set; }
    public DateTime? Ts { get; set; }
    public string Raw { get; set; } = "";
    public EntregaStatus Status { get; set; }
    public int Tentativas { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? EntregueEm { get; set; }
    public string? UltimoErro { get; set; }
}

// Histórico de mudança de status das bombas. Snapshot (não evento), então não é outbox:
// só registramos a mudança para auditoria/consulta. O envio segue no StatusPollingService.
public class StatusRegistroDb
{
    public int Id { get; set; }
    public string StatusChave { get; set; } = "";  // primeiros 33 chars (status real, sem versão/checksum)
    public string StatusFull { get; set; } = "";
    public DateTime CriadoEm { get; set; }
}
