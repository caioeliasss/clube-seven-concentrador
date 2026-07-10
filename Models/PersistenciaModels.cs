namespace SevenConcentradorBridge.Models;

// Registro de abastecimento persistido localmente. Fluxo pull: a bridge NÃO envia mais nada
// ao backend — ela acumula as vendas aqui e o backend consulta via API (busca por bico +
// horário). Cada venda é gravada uma vez, quando lida do concentrador (C_GetSale/C_NextSale).
public class AbastecimentoRegistroDb
{
    public int Id { get; set; }
    public string Bico { get; set; } = "";
    public decimal Volume { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorPorLitro { get; set; }
    public DateTime? Ts { get; set; }       // horário da venda (dia/hora/min/mês do registro, ano assumido)
    public string Raw { get; set; } = "";
    public DateTime CriadoEm { get; set; }  // quando a bridge gravou (base para a poda por retenção)
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
