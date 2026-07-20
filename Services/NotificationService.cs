using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;

namespace SevenConcentradorBridge.Services;

// Envia e-mail quando há problema com o concentrador (perda de conexão, crash da DLL).
// Config-driven pela seção "Notificacao" do appsettings.json. SMTP via System.Net.Mail.
//
// Anti-flood: cada problema tem uma "chave" (ex. "conexao"). Enquanto o problema persiste,
// o polling chama NotificarProblemaAsync a cada ciclo, mas o e-mail só é reenviado após o
// cooldown (Notificacao:CooldownMinutos). A recuperação (NotificarRecuperacaoAsync) só
// dispara e-mail se um problema daquela chave estava ativo — evita "tudo ok" sem alarme prévio.
public class NotificationService
{
    private readonly IConfiguration _config;
    private readonly ILogger<NotificationService> _logger;

    // chave -> último envio (para cooldown).
    private readonly ConcurrentDictionary<string, DateTime> _ultimoEnvio = new();
    // chaves com problema atualmente ativo (para disparar recuperação).
    private readonly ConcurrentDictionary<string, byte> _ativos = new();

    public NotificationService(IConfiguration config, ILogger<NotificationService> logger)
    {
        _config = config;
        _logger = logger;
    }

    private bool Habilitado =>
        bool.TryParse(_config["Notificacao:Habilitado"], out var b) && b;

    public async Task NotificarProblemaAsync(string chave, string assunto, string corpo)
    {
        bool novo = _ativos.TryAdd(chave, 1);

        var cooldown = TimeSpan.FromMinutes(
            int.TryParse(_config["Notificacao:CooldownMinutos"], out var m) ? m : 15);

        // Reenvia só se nunca enviou ou se já passou o cooldown desde o último envio.
        if (!novo
            && _ultimoEnvio.TryGetValue(chave, out var ultimo)
            && DateTime.UtcNow - ultimo < cooldown)
            return;

        _ultimoEnvio[chave] = DateTime.UtcNow;
        await EnviarAsync($"[Concentrador] {assunto}", corpo);
    }

    public async Task NotificarRecuperacaoAsync(string chave, string assunto, string corpo)
    {
        // Só avisa recuperação se havia problema ativo daquela chave.
        if (!_ativos.TryRemove(chave, out _)) return;
        _ultimoEnvio.TryRemove(chave, out _);
        await EnviarAsync($"[Concentrador] {assunto}", corpo);
    }

    private async Task EnviarAsync(string assunto, string corpo)
    {
        if (!Habilitado)
        {
            _logger.LogWarning("Notificação por e-mail desabilitada — não enviado: {Assunto}", assunto);
            return;
        }

        var host = _config["Notificacao:SmtpHost"] ?? "";
        var porta = int.TryParse(_config["Notificacao:SmtpPorta"], out var p) ? p : 587;
        var usuario = _config["Notificacao:SmtpUsuario"] ?? "";
        var senha = _config["Notificacao:SmtpSenha"] ?? "";
        var de = _config["Notificacao:De"] ?? usuario;
        var para = _config["Notificacao:Para"] ?? "";
        var ssl = !bool.TryParse(_config["Notificacao:SmtpSsl"], out var s) || s; // default true (STARTTLS)

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(para))
        {
            _logger.LogWarning("Notificacao:SmtpHost/Para não configurados — e-mail não enviado: {Assunto}", assunto);
            return;
        }

        try
        {
            using var client = new SmtpClient(host, porta)
            {
                EnableSsl = ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };
            if (!string.IsNullOrEmpty(usuario))
                client.Credentials = new NetworkCredential(usuario, senha);

            using var msg = new MailMessage
            {
                From = new MailAddress(string.IsNullOrWhiteSpace(de) ? usuario : de),
                Subject = assunto,
                Body = $"{corpo}\n\nHorário: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            };
            foreach (var dest in para.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                msg.To.Add(dest.Trim());

            await client.SendMailAsync(msg);
            _logger.LogInformation("E-mail de notificação enviado: {Assunto}", assunto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar e-mail de notificação: {Assunto}", assunto);
        }
    }
}
