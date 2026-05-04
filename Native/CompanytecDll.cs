using System.Runtime.InteropServices;

namespace SevenConcentradorBridge.Native;

public static class CompanytecDll
{
    private const string DllName = "companytec.dll";

    // === Comunicação ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int C_OpenSerial(int np);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int C_CloseSerial();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_OpenSocket([MarshalAs(UnmanagedType.LPStr)] string ip);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_OpenSocket2([MarshalAs(UnmanagedType.LPStr)] string ip, int port);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int C_CloseSocket();

    // === Abastecimento ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_GetSale();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_GetSalePAF();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void C_NextSale();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern IntPtr C_ReadRegister([MarshalAs(UnmanagedType.LPStr)] string reg);

    // === Visualização ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_Visualize();

    // === Status ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_readState();

    // === Gerenciamento de Bombas ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_FreePump([MarshalAs(UnmanagedType.LPStr)] string bico);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_AutoPump([MarshalAs(UnmanagedType.LPStr)] string bico);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_BlockPump([MarshalAs(UnmanagedType.LPStr)] string bico);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern IntPtr C_StopPump([MarshalAs(UnmanagedType.LPStr)] string bico);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_SetPrice(
        [MarshalAs(UnmanagedType.LPStr)] string bico,
        [MarshalAs(UnmanagedType.LPStr)] string preco);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_PresetPump(
        [MarshalAs(UnmanagedType.LPStr)] string bico,
        [MarshalAs(UnmanagedType.LPStr)] string cash);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_ReadTotalsVolume([MarshalAs(UnmanagedType.LPStr)] string bico);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_ReadTotalsCash([MarshalAs(UnmanagedType.LPStr)] string bico);

    // === Relógio ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_GetClock();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool C_SetClock([MarshalAs(UnmanagedType.LPStr)] string par);

    // === Identfid ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_ReadIdf();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void C_IncrementIdf();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_SaveTagIdf(int control1, int control2,
        [MarshalAs(UnmanagedType.LPStr)] string tag);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_DeleteTagIdf(
        [MarshalAs(UnmanagedType.LPStr)] string control,
        [MarshalAs(UnmanagedType.LPStr)] string position,
        [MarshalAs(UnmanagedType.LPStr)] string tag);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void C_ClearMemoryIdf();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_PushIdfBlackList([MarshalAs(UnmanagedType.LPStr)] string tag);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int C_RemoveIdfBlackList([MarshalAs(UnmanagedType.LPStr)] string tag);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr C_ReadRegisterIdf(int nro);

    // === Preço por litro (seção 2.6.6) ===

    [StructLayout(LayoutKind.Sequential)]
    public struct PPLNivel
    {
        public double Nivel0 { get; set; }
        public double Nivel1 { get; set; }
        public double Nivel2 { get; set; }
    }

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern IntPtr LePPLNivel(
        [MarshalAs(UnmanagedType.LPStr)] string bico,
        int niveis);

    // === Comando nativo ===

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern IntPtr C_SendReceiveText([MarshalAs(UnmanagedType.LPStr)] string comando);

    public static string PtrToString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero) return "";
        return Marshal.PtrToStringAnsi(ptr) ?? "";
    }

    public static string PtrToStringAnsi(IntPtr ptr) => PtrToString(ptr);
}
