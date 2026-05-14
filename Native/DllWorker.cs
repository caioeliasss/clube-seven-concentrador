using System.Text.Json;

namespace SevenConcentradorBridge.Native;

public static class DllWorker
{
    public static void Run()
    {
        Log($"WORKER START pid={Environment.ProcessId} logPath={LogPath}");
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            string responseJson;
            try
            {
                using var doc = JsonDocument.Parse(line);
                var root = doc.RootElement;
                var method = root.GetProperty("m").GetString()!;
                var args = root.TryGetProperty("a", out var argsEl)
                    ? argsEl.EnumerateArray().ToArray()
                    : Array.Empty<JsonElement>();

                Log($"REQ m={method} args={line}");
                var result = Dispatch(method, args);
                responseJson = "{\"r\":" + JsonSerializer.Serialize(result) + "}";
                Log($"RES {responseJson}");
            }
            catch (Exception ex)
            {
                responseJson = "{\"e\":" + JsonSerializer.Serialize(ex.Message) + "}";
                Log($"ERR {ex.GetType().Name}: {ex.Message}");
            }

            Console.WriteLine(responseJson);
        }
    }

    private static object? Dispatch(string method, JsonElement[] args) => method switch
    {
        "C_OpenSerial"      => CompanytecDll.C_OpenSerial(args[0].GetInt32()),
        "C_OpenSocket2"     => CompanytecDll.C_OpenSocket2(args[0].GetString()!, args[1].GetInt32()),
        "C_CloseSerial"     => CompanytecDll.C_CloseSerial(),
        "C_CloseSocket"     => CompanytecDll.C_CloseSocket(),
        "C_PresetPump"      => CompanytecDll.C_PresetPump(args[0].GetString()!, args[1].GetString()!),
        "C_readState"       => CompanytecDll.PtrToString(CompanytecDll.C_readState()),
        "C_GetSale"         => CompanytecDll.PtrToString(CompanytecDll.C_GetSale()),
        "C_GetSalePAF"      => CompanytecDll.PtrToString(CompanytecDll.C_GetSalePAF()),
        "C_NextSale"        => VoidCall(CompanytecDll.C_NextSale),
        "C_FreePump"        => CompanytecDll.C_FreePump(args[0].GetString()!),
        "C_BlockPump"       => CompanytecDll.C_BlockPump(args[0].GetString()!),
        "C_AutoPump"        => CompanytecDll.C_AutoPump(args[0].GetString()!),
        "C_Visualize"       => CompanytecDll.PtrToString(CompanytecDll.C_Visualize()),
        "C_SendReceiveText" => CompanytecDll.PtrToString(CompanytecDll.C_SendReceiveText(args[0].GetString()!)),
        "LePPLNivel"        => CallLePPLNivel(args[0].GetString()!, args[1].GetInt32()),
        _ => throw new InvalidOperationException($"Unknown method: {method}")
    };

    private static object? VoidCall(Action fn) { fn(); return null; }

    private static readonly string LogPath =
        Path.Combine(Path.GetTempPath(), "seven-dll-worker.log");

    private static void Log(string msg)
    {
        try { File.AppendAllText(LogPath, $"{DateTime.Now:O} {msg}\n"); } catch { }
    }

    private static CompanytecDll.PPLNivel? CallLePPLNivel(string bico, int niveis)
    {
        Log($"LePPLNivel ENTER bico='{bico}' niveis={niveis}");
        try
        {
            CompanytecDll.LePPLNivel(bico, niveis, out var res);
            Log($"LePPLNivel OK n0={res.Nivel0} n1={res.Nivel1} n2={res.Nivel2}");
            if (res.Nivel0 == -1.0) return null;
            return res;
        }
        catch (Exception ex)
        {
            Log($"LePPLNivel EX {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}
