using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SevenConcentradorBridge.Services;

/// <summary>
/// Ícone na bandeja do Windows (perto do relógio) enquanto a bridge roda.
/// Só usado no exe publicado (WinExe, sem console). Em dev mantém o console.
/// </summary>
public static class TrayIcon
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    /// <summary>True quando há janela de console (modo dev). Falso no exe WinExe publicado.</summary>
    public static bool ConsolePresent => GetConsoleWindow() != IntPtr.Zero;

    /// <summary>
    /// Inicia o host web e roda um loop de mensagens WinForms com o NotifyIcon.
    /// Bloqueia até o usuário clicar em "Sair" no menu da bandeja.
    /// </summary>
    public static void Run(WebApplication app, string porta)
    {
        // Sobe o host em background; o loop do tray segura o processo.
        app.Start();

        var thread = new Thread(() =>
        {
            Application.EnableVisualStyles();

            using var notify = new NotifyIcon();
            var icoPath = Path.Combine(AppContext.BaseDirectory, "seven-logo.ico");
            notify.Icon = File.Exists(icoPath) ? new Icon(icoPath) : SystemIcons.Application;
            notify.Text = $"Seven Concentrador Bridge — porta {porta}";
            notify.Visible = true;

            var menu = new ContextMenuStrip();
            menu.Items.Add("Abrir painel", null, (_, _) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo($"http://localhost:{porta}") { UseShellExecute = true });
                }
                catch { /* navegador indisponível — ignora */ }
            });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Sair", null, (_, _) => Application.ExitThread());
            notify.ContextMenuStrip = menu;

            // Duplo clique abre o painel também.
            notify.DoubleClick += (_, _) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo($"http://localhost:{porta}") { UseShellExecute = true });
                }
                catch { /* ignora */ }
            };

            Application.Run();

            notify.Visible = false;
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        app.StopAsync().GetAwaiter().GetResult();
    }
}
