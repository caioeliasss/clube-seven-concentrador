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
        "VB_SendReceiveText" => CallVbSendReceiveText(args[0].GetString()!, args.Length > 1 ? (short)args[1].GetInt32() : (short)2000),
        "SendReceiveText" => CallSendReceiveText(args[0].GetString()!, args.Length > 1 ? args[1].GetInt32() : 2000),
        "ReadPriceLiterLevel0" => CallReadPriceLiterLevel0(args[0].GetString()!),
        _ => throw new InvalidOperationException($"Unknown method: {method}")
    };

    private static object? VoidCall(Action fn) { fn(); return null; }

    private static readonly string LogPath =
        Path.Combine(Path.GetTempPath(), "seven-dll-worker.log");

    private static void Log(string msg)
    {
        try { File.AppendAllText(LogPath, $"{DateTime.Now:O} {msg}\n"); } catch { }
    }

    private static string CallVbSendReceiveText(string comando, short timeout)
    {
        Log($"VB_SendReceiveText ENTER cmd='{comando}' timeout={timeout}");
        try
        {
            var buf = new byte[512];
            var data = System.Text.Encoding.ASCII.GetBytes(comando);
            if (data.Length >= buf.Length)
                throw new InvalidOperationException($"Comando excede buffer ({data.Length} >= {buf.Length})");
            Array.Copy(data, buf, data.Length);

            short len = CompanytecDll.VB_SendReceiveText(buf, timeout);
            Log($"VB_SendReceiveText ret len={len}");
            if (len <= 0) return "";

            int safeLen = Math.Min((int)len, buf.Length);
            return System.Text.Encoding.ASCII.GetString(buf, 0, safeLen);
        }
        catch (Exception ex)
        {
            Log($"VB_SendReceiveText EX {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    private static string CallSendReceiveText(string comando, int timeout)
    {
        Log($"SendReceiveText ENTER cmd='{comando}' timeout={timeout}");
        const int BufSize = 1024;
        IntPtr buf = System.Runtime.InteropServices.Marshal.AllocHGlobal(BufSize);
        try
        {
            for (int i = 0; i < BufSize; i++)
                System.Runtime.InteropServices.Marshal.WriteByte(buf, i, 0);

            var bytes = System.Text.Encoding.ASCII.GetBytes(comando);
            if (bytes.Length >= BufSize)
                throw new InvalidOperationException($"Comando excede buffer ({bytes.Length} >= {BufSize})");

            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, buf, bytes.Length);

            IntPtr ptr = buf;
            Log($"SendReceiveText CALL bufBefore=0x{buf.ToInt64():X}");
            int len = CompanytecDll.SendReceiveText(ref ptr, timeout);
            Log($"SendReceiveText ret len={len} ptrAfter=0x{ptr.ToInt64():X} reapontou={(ptr != buf)}");

            string fromBuf = ReadAnsiZ(buf, BufSize);
            string fromPtr = (ptr != buf) ? ReadAnsiZ(ptr, BufSize) : fromBuf;
            Log($"SendReceiveText fromBuf='{fromBuf}' fromPtr='{fromPtr}'");

            string resp = !string.IsNullOrEmpty(fromPtr) ? fromPtr : fromBuf;
            if (!string.IsNullOrEmpty(resp)) return resp;

            if (len > 0)
            {
                int safeLen = Math.Min(len, BufSize);
                return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr, safeLen) ?? "";
            }
            return "";
        }
        catch (Exception ex)
        {
            Log($"SendReceiveText EX {ex.GetType().Name}: {ex.Message}");
            throw;
        }
        finally
        {
            System.Runtime.InteropServices.Marshal.FreeHGlobal(buf);
        }
    }

    private static string ReadAnsiZ(IntPtr ptr, int max)
    {
        if (ptr == IntPtr.Zero) return "";
        try
        {
            var sb = new System.Text.StringBuilder(64);
            for (int i = 0; i < max; i++)
            {
                byte b = System.Runtime.InteropServices.Marshal.ReadByte(ptr, i);
                if (b == 0) break;
                sb.Append((char)b);
            }
            return sb.ToString();
        }
        catch { return ""; }
    }

    private static int CallReadPriceLiterLevel0(string nozzle)
    {
        Log($"ReadPriceLiterLevel0 ENTER nozzle='{nozzle}'");
        try
        {
            int ret = CompanytecDll.C_ReadPriceLiterLevel0(nozzle);
            Log($"ReadPriceLiterLevel0 ret={ret}");
            return ret;
        }
        catch (Exception ex)
        {
            Log($"ReadPriceLiterLevel0 EX {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}
