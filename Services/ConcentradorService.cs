using System.Collections.Concurrent;
using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Native;

namespace SevenConcentradorBridge.Services;

public class ConcentradorService : IDisposable
{
    private readonly ILogger<ConcentradorService> _logger;
    private readonly IConfiguration _config;
    private bool _connected;

    private readonly BlockingCollection<Action> _fila = new();
    private readonly Thread _thread;

    public ConcentradorService(ILogger<ConcentradorService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;

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
            resultado = CompanytecDll.C_OpenSerial(porta);
        }
        else
        {
            var ip = _config["Concentrador:Ip"] ?? "192.168.0.2";
            var porta = int.Parse(_config["Concentrador:Porta"] ?? "2001");
            resultado = CompanytecDll.C_OpenSocket2(ip, porta);
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
            CompanytecDll.C_CloseSerial();
        else
            CompanytecDll.C_CloseSocket();
        _connected = false;
        _logger.LogInformation("Desconectado do concentrador");
    });

    public bool PresetarBomba(string bico, string valor) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        var result = CompanytecDll.C_PresetPump(bico, valor);
        _logger.LogInformation("Preset bico {Bico} valor {Valor}: {Result}", bico, valor, result);
        return result == 1;
    });

    public string LerStatus() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.PtrToString(CompanytecDll.C_readState());
    });

    public string LerAbastecimento() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.PtrToString(CompanytecDll.C_GetSale());
    });

    public string LerAbastecimentoPAF() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.PtrToString(CompanytecDll.C_GetSalePAF());
    });

    public void IncrementarPonteiro() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        CompanytecDll.C_NextSale();
    });

    public bool LiberarBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.C_FreePump(bico) == 1;
    });

    public bool BloquearBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.C_BlockPump(bico) == 1;
    });

    public bool AutorizarBico(string bico) => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.C_AutoPump(bico) == 1;
    });

    public string LerVisualizacao() => Executar(() =>
    {
        if (!_connected) throw new InvalidOperationException("Não conectado ao concentrador");
        return CompanytecDll.PtrToString(CompanytecDll.C_Visualize());
    });

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

            string resPrecos = EnviarComando($"05{bicoPad}09");
            if (string.IsNullOrEmpty(resPrecos) || IsRespostaErro(resPrecos))
                continue;

            var (n0, n1, n2) = ParsePrecos(resPrecos);

            resultado.Add(new PrecoCombustivel
            {
                Bico = bico,
                CodigoCombustivel = codCombustivel,
                Combustivel = GetNomeCombustivel(codCombustivel),
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

        string resPrecos = EnviarComando($"05{bicoPad}09");
        if (string.IsNullOrEmpty(resPrecos) || IsRespostaErro(resPrecos))
            return null;

        var (n0, n1, n2) = ParsePrecos(resPrecos);

        return new PrecoCombustivel
        {
            Bico = bico,
            CodigoCombustivel = codCombustivel,
            Combustivel = GetNomeCombustivel(codCombustivel),
            PrecoNivel0Raw = n0,
            PrecoNivel1Raw = n1,
            PrecoNivel2Raw = n2,
        };
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
        var ptr = CompanytecDll.C_SendReceiveText(comando);
        string resp = CompanytecDll.PtrToString(ptr);
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
    }
}
