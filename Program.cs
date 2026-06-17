using SevenConcentradorBridge.Native;
using SevenConcentradorBridge.Services;

if (args.Contains("--worker"))
{
    DllWorker.Run();
    return;
}

// Load .env.dev or .env.prod based on ASPNETCORE_ENVIRONMENT before builder
var aspnetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var envFile = aspnetEnv.Equals("Development", StringComparison.OrdinalIgnoreCase) ? ".env.development" : ".env.production";
foreach (var envPath in new[]
{
    Path.Combine(AppContext.BaseDirectory, envFile),
    Path.Combine(Directory.GetCurrentDirectory(), envFile),
})
{
    if (!File.Exists(envPath)) continue;
    foreach (var line in File.ReadAllLines(envPath))
    {
        var t = line.Trim();
        if (string.IsNullOrEmpty(t) || t.StartsWith('#')) continue;
        var eq = t.IndexOf('=');
        if (eq < 0) continue;
        Environment.SetEnvironmentVariable(t[..eq].Trim(), t[(eq + 1)..].Trim());
    }
    break;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient("Backend");

// Registrar serviços
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<ConcentradorService>();
builder.Services.AddSingleton<PollingService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<PollingService>());
builder.Services.AddHostedService<StatusPollingService>();

var app = builder.Build();

// Painel web servido em "/" (wwwroot/index.html). Antes do middleware de auth
// para a página carregar sem X-Api-Key; as chamadas de API seguem protegidas.
app.UseDefaultFiles();
app.UseStaticFiles();

// Middleware de autenticação por API Key
app.Use(async (context, next) =>
{
    // Health check e leitura de config (GET) liberados sem auth: o painel precisa
    // carregar os valores padrão já no primeiro acesso, antes de o operador colar a
    // X-Api-Key. Os segredos (Auth/Backend ApiKey) são mascarados na resposta quando
    // a chave não confere — ver LerConfig — então não vazam na LAN.
    var p = context.Request.Path;
    var ehGetConfig = HttpMethods.IsGet(context.Request.Method)
        && p.StartsWithSegments("/api/concentrador/config");
    if (p.StartsWithSegments("/api/concentrador/health") || ehGetConfig)
    {
        await next();
        return;
    }

    var apiKey = app.Configuration["Auth:ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        var requestKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (requestKey != apiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { erro = "API Key inválida" });
            return;
        }
    }

    await next();
});

app.MapControllers();

var porta = app.Configuration["Bridge:Porta"] ?? "5100";
app.Urls.Add($"http://0.0.0.0:{porta}");

// Exe publicado (WinExe, sem console): roda com ícone na bandeja perto do relógio.
// Dev (console presente): mantém comportamento normal com logs e Ctrl+C.
if (TrayIcon.ConsolePresent)
    app.Run();
else
    TrayIcon.Run(app, porta);
