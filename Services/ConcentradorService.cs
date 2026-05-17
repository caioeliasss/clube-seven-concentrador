using System.Collections.Concurrent;
using System.Globalization;
using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Native;

namespace SevenConcentradorBridge.Services;

public class ConcentradorService : IDisposable
{
    private readonly ILogger<ConcentradorService> _logger;
    private readonly IConfiguration _config;
    private readonly DllProxyClient _dll;
    private bool _connected;

    private readonly BlockingCollection<Action> _fila = new();
    private readonly Thread _thread;

    public ConcentradorService(ILogger<ConcentradorService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;

        _dll = new DllProxyClient();
        _dll.OnWorkerRestarted += () => _connected = false;

        _thread = new Thread(() =>
        {
            foreach (var acao in _fila.GetConsumingEnumerable())
                acao();
        });
        _thread.IsBackground = true;
        _thread.Name = "DLL-Concentrador";
        _thread.Start();
    }

    private T Executar<T>(Func<T> func)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        _fila.Add(() =>
        {
            try { tcs.SetResult(func()); }
            catch (Exception ex) { tcs.SetException(ex); }
        });
        return tcs.Task.GetAwaiter().GetResult();
    }

    private void Executar(Action action) => Executar<bool>(() => { action(); return true; });

    public bool Conectar() => Executar(() =>
    {
        if (_connected) return true;

        var tipo = _config["Concentrador:TipoConexao"] ?? "ethernet";
        int resultado;

        if (tipo == "serial")
        {
            var porta = int.Parse(_config["Concentrador:PortaSerial"] ?? "1");
            resultado = _dll.C_OpenSerial(porta);
        }
        else
        {
            var ip = _config["Concentrador:Ip"] ?? "192.168.0.2";
            var porta = int.Parse(_config["Concentrador:Porta"] ?? "2001");
            resultado = _dll.C_OpenSocket2(ip, porta);
        }

        _connected = resultado == 1;
        if (_connected)
            _logger.LogInformation("Conectado ao concentrador");
        else
            _logger.LogError("Falha ao conectar ao concentrador");

        return _connected;
    });

    public void Desconectar() => Executar(() =>
    {
        if (!_connected) return;
        var tipo = _config["Concentrador:TipoConexao"] ?? "ethernet";
        if (tipo == "serial")
            _dll.C_CloseSerial();
        else
            _dll.C_CloseSocket();
        _connected = false;
        _logger.LogInformation("Desconectado do concentrador");
    });

    public bool PresetarBomba(string bico, string valor) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        var result = _dll.C_PresetPump(bico, valor);
        _logger.LogInformation("Preset bico {Bico} valor {Valor}: {Result}", bico, valor, result);
        return result == 1;
    });

    // C_SetPrice (Manual §4.6.5): preço 4 dígitos × 1000 (3 decimais — ex. "5990" = R$5,990).
    // Aplicado no display somente no próximo abastecimento.
    public bool AlterarPreco(string bico, string preco) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        string bicoPad = bico.PadLeft(2, '0');
        string precoPad = preco.PadLeft(4, '0');
        var result = _dll.C_SetPrice(bicoPad, precoPad);
        _logger.LogInformation("SetPrice bico {Bico} preco {Preco}: {Result}", bicoPad, precoPad, result);
        return result == 1;
    });

    public string LerStatus() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_readState();
    });

    public string LerAbastecimento() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_GetSale();
    });

    public string LerAbastecimentoPAF() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_GetSalePAF();
    });

    public void IncrementarPonteiro() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        _dll.C_NextSale();
    });

    public bool LiberarBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_FreePump(bico) == 1;
    });

    public bool BloquearBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_BlockPump(bico) == 1;
    });

    public bool AutorizarBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_AutoPump(bico) == 1;
    });

    public string LerVisualizacao() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return _dll.C_Visualize();
    });

    // C_Visualize retorna "(BBPPPPPP...)" — N blocos de 8 chars (bico 2 + volume 6).
    // Manual §4.3.1: 48 posições, bico "00" = slot vazio. Volume é raw × 100 (2 decimais).
    public VisualizacaoResponse LerVisualizacaoParsed()
    {
        string raw = LerVisualizacao();
        var resp = new VisualizacaoResponse { Raw = raw };

        if (string.IsNullOrEmpty(raw)) return resp;

        string payload = raw.Trim();
        if (payload.StartsWith("(")) payload = payload[1..];
        if (payload.EndsWith(")")) payload = payload[..^1];

        for (int i = 0; i + 8 <= payload.Length; i += 8)
        {
            string bico = payload.Substring(i, 2);
            if (bico == "00") continue;

            string volRaw = payload.Substring(i + 2, 6);
            if (!int.TryParse(volRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int volInt))
                continue;

            resp.Bicos.Add(new VisualizacaoBico
            {
                Bico = bico,
                VolumeRaw = volRaw,
                VolumeLitros = volInt / 100m,
            });
        }

        return resp;
    }

    public List<PrecoCombustivel> LerPrecosTodos() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");

        var resultado = new List<PrecoCombustivel>();

        // Bicos em decimal (01-32). Protocolo exige decimal — valores hex causam crash na DLL.
        var possibleBicos = Enumerable.Range(1, 32).Select(n => n.ToString("D2"));

        foreach (var bico in possibleBicos)
        {
            string bicoPad = bico;

            string resTabela = EnviarComando($"14{bicoPad}");
            if (string.IsNullOrEmpty(resTabela) || IsRespostaErro(resTabela))
                continue;

            int codCombustivel = ParseCodCombustivel(resTabela);
            if (codCombustivel == 0) continue;

            string resPrecoUnitario = EnviarComando($"05{bicoPad}03");
            var (precoAtual, precoAnterior) = (!string.IsNullOrEmpty(resPrecoUnitario) && !IsRespostaErro(resPrecoUnitario))
                ? ParsePrecoUnitario(resPrecoUnitario)
                : ("", "");

            string resPrecos = EnviarComando($"05{bicoPad}09");
            if (string.IsNullOrEmpty(resPrecos) || IsRespostaErro(resPrecos))
                continue;

            var (n0, n1, n2) = ParsePrecos(resPrecos);

            resultado.Add(new PrecoCombustivel
            {
                Bico = bico,
                CodigoCombustivel = codCombustivel,
                Combustivel = GetNomeCombustivel(codCombustivel),
                PrecoAtualRaw = precoAtual,
                PrecoAnteriorRaw = precoAnterior,
                PrecoNivel0Raw = n0,
                PrecoNivel1Raw = n1,
                PrecoNivel2Raw = n2,
            });
        }

        return resultado;
    });

    public PrecoCombustivel? LerPrecoPorBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");

        string bicoPad = bico.PadLeft(2, '0');

        string resTabela = EnviarComando($"14{bicoPad}");
        if (string.IsNullOrEmpty(resTabela) || IsRespostaErro(resTabela))
            return null;

        int codCombustivel = ParseCodCombustivel(resTabela);

        string resPrecoUnitario = EnviarComando($"05{bicoPad}03");
        var (precoAtual, precoAnterior) = (!string.IsNullOrEmpty(resPrecoUnitario) && !IsRespostaErro(resPrecoUnitario))
            ? ParsePrecoUnitario(resPrecoUnitario)
            : ("", "");

        string resPrecos = EnviarComando($"05{bicoPad}09");
        if (string.IsNullOrEmpty(resPrecos) || IsRespostaErro(resPrecos))
            return null;

        var (n0, n1, n2) = ParsePrecos(resPrecos);

        return new PrecoCombustivel
        {
            Bico = bico,
            CodigoCombustivel = codCombustivel,
            Combustivel = GetNomeCombustivel(codCombustivel),
            PrecoAtualRaw = precoAtual,
            PrecoAnteriorRaw = precoAnterior,
            PrecoNivel0Raw = n0,
            PrecoNivel1Raw = n1,
            PrecoNivel2Raw = n2,
        };
    });

    public PrecoPorLitro? LerPrecoDllPorBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");

        string bicoPad = bico.PadLeft(2, '0');
        int raw = _dll.ReadPriceLiterLevel0(bicoPad);
        if (raw < 0) return null;

        return new PrecoPorLitro
        {
            Bico = bico,
            Sucesso = true,
            Nivel0 = (raw / 1000m).ToString("F3", CultureInfo.InvariantCulture),
            Raw = raw.ToString(CultureInfo.InvariantCulture),
        };
    });

    public List<PrecoPorLitro> LerPrecosDll() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");

        var resultado = new List<PrecoPorLitro>();

        foreach (var n in Enumerable.Range(4, 5))
        {
            string bicoPad = n.ToString("D2");
            int raw = _dll.ReadPriceLiterLevel0(bicoPad);
            if (raw < 0) continue;

            resultado.Add(new PrecoPorLitro
            {
                Bico = bicoPad,
                Sucesso = true,
                Nivel0 = (raw / 1000m).ToString("F3", CultureInfo.InvariantCulture),
                Raw = raw.ToString(CultureInfo.InvariantCulture),
            });
        }

        return resultado;
    });

    // Protocolo RS-232 "AdicionaCheck": (corpo + checksum). checksum = (sum chars) & 0xFF em hex.
    // Exemplo bico 04: TX "(&T04U33)" -> RX "(TG04000000007F)"
    public (string comando, string resposta) LerPrecoUnitarioRaw(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");

        string bicoPad = bico.PadLeft(2, '0');
        string body = $"&T{bicoPad}U33";
        int sum = 0;
        foreach (char c in body) sum += c;
        // string comando = $"({body}{(sum & 0xFF):X2})";
        string comando = $"({body})";

        _logger.LogDebug("TX (raw): {Cmd}", comando);
        string resp = _dll.C_SendReceiveText(comando);
        _logger.LogDebug("RX (raw): {Resp}", resp);
        return (comando, resp);
    });

    // §3.1.10: (&T99PKK) → (TP99XXXXYYYYKK) — ler ponteiros sem incrementar.
    public PonteirosResponse LerPonteiros() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        string raw = EnviarRawCompanytec("&T99P");
        return ParsePonteiros(raw);
    });

    // §3.1.9: (&LRXXXXKK) → registro PAF1 da posição. NÃO incrementa ponteiro global.
    public AbastecimentoRegistro LerRegistro(int posicao) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        if (posicao < 0 || posicao > 9999)
            throw new ArgumentOutOfRangeException(nameof(posicao), "Posição deve estar entre 0-9999");
        string body = $"&LR{posicao:D4}";
        string raw = EnviarRawCompanytec(body);
        return ParseRegistro(raw, posicao);
    });

    private string EnviarRawCompanytec(string body)
    {
        int sum = 0;
        foreach (char c in body) sum += c;
        string checksum = (sum & 0xFF).ToString("X2");
        string comando = $"({body}{checksum})";
        _logger.LogDebug("TX raw: {Cmd}", comando);
        string resp = _dll.SendReceiveText(comando);
        _logger.LogDebug("RX raw: {Resp}", resp);
        return resp;
    }

    private static string StripParens(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        var t = s.Trim();
        if (t.StartsWith("(")) t = t[1..];
        if (t.EndsWith(")")) t = t[..^1];
        return t;
    }

    private static PonteirosResponse ParsePonteiros(string raw)
    {
        var resp = new PonteirosResponse { Raw = raw };
        string payload = StripParens(raw);
        if (payload.Length < 12 || !payload.StartsWith("TP99")) return resp;

        if (int.TryParse(payload.Substring(4, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out int w))
            resp.Write = w;
        if (int.TryParse(payload.Substring(8, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out int r))
            resp.Read = r;

        // Fila circular — assume capacidade máx 10000 posições (ajustar se concentrador diferir).
        resp.Pendentes = resp.Write >= resp.Read
            ? resp.Write - resp.Read
            : (10000 + resp.Write - resp.Read);
        resp.Valido = true;
        return resp;
    }

    private static AbastecimentoRegistro ParseRegistro(string raw, int posicao)
    {
        var reg = new AbastecimentoRegistro { Raw = raw, PosicaoConsultada = posicao };
        string payload = StripParens(raw);

        // Posição vazia: protocolo retorna string com FFFFF...
        if (string.IsNullOrEmpty(payload) || payload.StartsWith("FFFFFF"))
        {
            reg.Vazio = true;
            return reg;
        }

        // CBC04/05/06: TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEEFFIIIIIIIIIIIIIIIINNNNSSKK
        if (payload.Length < 50) return reg;

        reg.TotalRaw = payload.Substring(0, 6);
        reg.LitrosRaw = payload.Substring(6, 6);
        reg.PrecoUnitarioRaw = payload.Substring(12, 4);
        reg.CodigoVirgula = payload.Substring(16, 2);
        reg.Bico = payload.Substring(22, 2);
        reg.Dia = payload.Substring(24, 2);
        reg.Hora = payload.Substring(26, 2);
        reg.Minuto = payload.Substring(28, 2);
        reg.Mes = payload.Substring(30, 2);

        if (int.TryParse(payload.Substring(32, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out int regNum))
            reg.Registro = regNum;

        reg.TotalizadorFinalRaw = payload.Substring(36, 10);
        if (payload.Length >= 64)
            reg.Identificador = payload.Substring(48, 16);

        // Conversão depende do código de vírgula (protocolo §3.7). Default: total /100 (R$ 2 dec), litros /1000 (3 dec).
        if (decimal.TryParse(reg.TotalRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var t))
            reg.TotalReais = t / 100m;
        if (decimal.TryParse(reg.LitrosRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l))
            reg.Litros = l / 1000m;

        reg.Vazio = reg.TotalReais == 0 && reg.Litros == 0;
        return reg;
    }

    // Envia comando nativo no formato Companytec (ex. "(&T04U33)" — protocolo 3.5.4).
    // Usa export SendReceiveText (dllcompanytec.pas:346):
    //   Function SendReceiveText(var st: PAnsiChar; timeout: integer): integer; stdcall;
    // ABI C-friendly (PAnsiChar*), evita Delphi ShortString do C_SendReceiveText e
    // double-indirection ByRef Byte() do VB_SendReceiveText.
    public (string comando, string resposta) EnviarNativo(string comando) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        _logger.LogDebug("TX (nativo): {Cmd}", comando);
        string resposta = _dll.SendReceiveText(comando);
        _logger.LogDebug("RX (nativo): {Resp}", resposta);
        return (comando, resposta);
    });


    private string EnviarComando(string dataHex)
    {
        string sizeHex = dataHex.Length.ToString("X4");
        string payload = sizeHex + dataHex;
        int sum = '?';
        foreach (char c in payload)
            sum += c;
        string checksum = (sum % 256).ToString("X2");
        string comando = $">?{payload}{checksum}";
        _logger.LogDebug("TX: {Cmd}", comando);
        string resp = _dll.C_SendReceiveText(comando);
        _logger.LogDebug("RX: {Resp}", resp);
        return resp;
    }

    private static bool IsRespostaErro(string response)
    {
        return response.Length < 8 || response.Substring(2, 4) == "0004";
    }

    private static int ParseCodCombustivel(string response)
    {
        if (response.Length < 20) return 0;
        return Convert.ToInt32(response.Substring(18, 2), 16);
    }

    private static (string n0, string n1, string n2) ParsePrecos(string response)
    {
        if (response.Length < 30) return ("", "", "");
        return (
            response.Substring(12, 6),
            response.Substring(18, 6),
            response.Substring(24, 6)
        );
    }

    // TT=03: >!CCCC05BBTTATUAANTER KK — preço atual (4) + anterior (4) a partir de pos 12
    private static (string atual, string anterior) ParsePrecoUnitario(string response)
    {
        if (response.Length < 20) return ("", "");
        return (response.Substring(12, 4), response.Substring(16, 4));
    }

    private static string GetNomeCombustivel(int codigo) => codigo switch
    {
        1 => "Gasolina Comum",
        2 => "Gasolina Aditivada",
        3 => "Gasolina Premium",
        4 => "Gasolina Fórmula",
        5 => "Gasolina Podium",
        9 => "Gasolina V-Power",
        10 => "Diesel",
        11 => "Diesel Aditivado",
        13 => "Diesel S50",
        14 => "Diesel Maxxi",
        17 => "GNV",
        19 => "Etanol",
        20 => "Óleo Lubrificante",
        _ => $"Combustível {codigo}",
    };

    public void Dispose()
    {
        _fila.CompleteAdding();
        _thread.Join(TimeSpan.FromSeconds(5));
        _fila.Dispose();
        _dll.Dispose();
    }
}
