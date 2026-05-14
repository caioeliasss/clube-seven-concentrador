using System.Text.Json;

namespace SevenConcentradorBridge.Native;

public static class DllWorker
{
    public static void Run()
    {
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

                var result = Dispatch(method, args);
                responseJson = "{\"r\":" + JsonSerializer.Serialize(result) + "}";
            }
            catch (Exception ex)
            {
                responseJson = "{\"e\":" + JsonSerializer.Serialize(ex.Message) + "}";
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

    private static CompanytecDll.PPLNivel? CallLePPLNivel(string bico, int niveis)
    {
        CompanytecDll.LePPLNivel(bico, niveis, out var res);
        if (res.Nivel0 == 0 && res.Nivel1 == 0 && res.Nivel2 == 0) return null;
        return res;
    }
}
