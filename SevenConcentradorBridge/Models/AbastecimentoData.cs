namespace SevenConcentradorBridge.Models;

/// <summary>Dados de abastecimento retornados pelo concentrador.</summary>
public class AbastecimentoData
{
    public bool Valido { get; set; }
    public decimal TotalDinheiro { get; set; }
    public double TotalLitros { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string Tempo { get; set; } = "";
    public string Bico { get; set; } = "";
    public string Data { get; set; } = "";
    public string Hora { get; set; } = "";
    public int Registro { get; set; }
    public double Encerrante { get; set; }
}

/// <summary>Status possíveis de um bico.</summary>
public enum StatusBico
{
    Desconhecido,
    Livre,
    Abastecendo,
    Bloqueado,
    Finalizado
}

/// <summary>Status de um bico individual.</summary>
public class BicoStatus
{
    public string Bico { get; set; } = "";
    public StatusBico Status { get; set; }
    public double? VolumeAtual { get; set; }
}
