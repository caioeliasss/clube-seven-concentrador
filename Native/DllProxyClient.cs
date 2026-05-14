using System.Diagnostics;
using System.Text.Json;

namespace SevenConcentradorBridge.Native;

public class DllProxyClient : IDisposable
{
    private Process? _worker;
    private bool _started;
    private bool _disposed;

    public event Action? OnWorkerRestarted;

    public DllProxyClient() => Restart();

    private void Restart()
    {
        Kill();
        var (file, workerArgs) = GetWorkerStartInfo();
        _worker = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = file,
                Arguments = workerArgs,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = AppContext.BaseDirectory,
            }
        };
        _worker.Start();
        if (_started) OnWorkerRestarted?.Invoke();
        _started = true;
    }

    private static (string file, string arguments) GetWorkerStartInfo()
    {
        var processExe = Environment.ProcessPath!;
        if (Path.GetFileNameWithoutExtension(processExe)
                .Equals("dotnet", StringComparison.OrdinalIgnoreCase))
        {
            var name = typeof(DllProxyClient).Assembly.GetName().Name!;
            var dllPath = Path.Combine(AppContext.BaseDirectory, name + ".dll");
            return (processExe, $"\"{dllPath}\" --worker");
        }
        return (processExe, "--worker");
    }

    private string? Send(string method, params object?[] args)
    {
        if (_worker == null || _worker.HasExited)
            Restart();

        var reqArgs = args.Length > 0 ? ",\"a\":" + JsonSerializer.Serialize(args) : "";
        var req = $"{{\"m\":{JsonSerializer.Serialize(method)}{reqArgs}}}";

        try
        {
            _worker!.StandardInput.WriteLine(req);
            _worker.StandardInput.Flush();

            var line = _worker.StandardOutput.ReadLine();
            if (line == null)
            {
                int? exitCode = null;
                try { if (_worker.HasExited) exitCode = _worker.ExitCode; } catch { }
                Restart();
                throw new InvalidOperationException(
                    $"DLL worker crashed durante '{method}' (exit code {exitCode?.ToString() ?? "?"}). " +
                    "Provável Access Violation na DLL nativa. Confira args/marshalling. " +
                    "Log: %TEMP%\\seven-dll-worker.log");
            }

            using var doc = JsonDocument.Parse(line);
            var root = doc.RootElement;
            if (root.TryGetProperty("e", out var err))
                throw new InvalidOperationException(err.GetString());
            if (root.TryGetProperty("r", out var result))
                return result.ValueKind == JsonValueKind.Null ? null : result.ToString();
            return null;
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            Restart();
            throw new InvalidOperationException(
                $"Falha na comunicação com DLL worker durante '{method}': {ex.GetType().Name}: {ex.Message}", ex);
        }
    }

    private int SendInt(string method, params object?[] args) =>
        int.TryParse(Send(method, args), out var v) ? v : 0;

    private string SendStr(string method, params object?[] args) =>
        Send(method, args) ?? "";

    public int C_OpenSerial(int np)                        => SendInt("C_OpenSerial", np);
    public int C_OpenSocket2(string ip, int port)          => SendInt("C_OpenSocket2", ip, port);
    public int C_CloseSerial()                             => SendInt("C_CloseSerial");
    public int C_CloseSocket()                             => SendInt("C_CloseSocket");
    public int C_PresetPump(string bico, string valor)     => SendInt("C_PresetPump", bico, valor);
    public string C_readState()                            => SendStr("C_readState");
    public string C_GetSale()                              => SendStr("C_GetSale");
    public string C_GetSalePAF()                           => SendStr("C_GetSalePAF");
    public void C_NextSale()                               => Send("C_NextSale");
    public int C_FreePump(string bico)                     => SendInt("C_FreePump", bico);
    public int C_BlockPump(string bico)                    => SendInt("C_BlockPump", bico);
    public int C_AutoPump(string bico)                     => SendInt("C_AutoPump", bico);
    public string C_Visualize()                            => SendStr("C_Visualize");
    public string C_SendReceiveText(string cmd)            => SendStr("C_SendReceiveText", cmd);
    public int ReadPriceLiterLevel0(string nozzle) =>
        SendInt("ReadPriceLiterLevel0", nozzle);

    private void Kill()
    {
        if (_worker == null) return;
        try { if (!_worker.HasExited) _worker.Kill(); } catch { }
        _worker.Dispose();
        _worker = null;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Kill();
    }
}
