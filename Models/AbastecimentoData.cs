namespace SevenConcentradorBridge.Models;

public class AbastecimentoData
{
    public bool Value { get; set; }
    public decimal TotalDinheiro { get; set; }
    public double TotalLitros { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string Tempo { get; set; } = "";
    public string Canal { get; set; } = "";
    public string Data { get; set; } = "";
    public string Hora { get; set; } = "";
    public string StFull { get; set; } = "";
    public int Registro { get; set; }
    public double Encerrante { get; set; }
    public bool Integridade { get; set; }
    public bool Checksum { get; set; }
}

public class AbastecimentoPAF1Data
{
    public bool Value { get; set; }
    public decimal TotalDinheiro { get; set; }
    public double TotalLitros { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string Tempo { get; set; } = "";
    public string CodBico { get; set; } = "";
    public int NumBico { get; set; }
    public int NumTanque { get; set; }
    public int VolTanque { get; set; }
    public int CodCombustivel { get; set; }
    public int SerieCbc { get; set; }
    public char TipoCbc { get; set; }
    public string Data { get; set; } = "";
    public string Hora { get; set; } = "";
    public int Registro { get; set; }
    public double EncerranteI { get; set; }
    public double EncerranteF { get; set; }
    public bool Integridade { get; set; }
    public bool Checksum { get; set; }
    public string Tag1 { get; set; } = "";
    public string Tag2 { get; set; } = "";
}

public class StatusBico
{
    public string Bico { get; set; } = "";
    public string Status { get; set; } = "";
}

public class PresetRequest
{
    public string Bico { get; set; } = "";
    public string Valor { get; set; } = "";
}

public class BicoRequest
{
    public string Bico { get; set; } = "";
}

public class NativeCommandRequest
{
    public string Comando { get; set; } = "";
}


public class PrecoCombustivel
{
    public string Bico { get; set; } = "";
    public int CodigoCombustivel { get; set; }
    public string Combustivel { get; set; } = "";
    public string PrecoAtualRaw { get; set; } = "";
    public string PrecoAnteriorRaw { get; set; } = "";
    public string PrecoNivel0Raw { get; set; } = "";
    public string PrecoNivel1Raw { get; set; } = "";
    public string PrecoNivel2Raw { get; set; } = "";
}

public class PrecoPorLitro
{
    public string Bico { get; set; } = "";
    public bool Sucesso { get; set; }
    public string? Nivel0 { get; set; }
    public string? Nivel1 { get; set; }
    public string? Nivel2 { get; set; }
    public string? Raw { get; set; }
}

public class WebhookPayload
{
    public string IdConcentrador { get; set; } = "";
    public string Bico { get; set; } = "";
    public decimal TotalDinheiro { get; set; }
    public double TotalLitros { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string Status { get; set; } = "aguardando_pagamento";
}
