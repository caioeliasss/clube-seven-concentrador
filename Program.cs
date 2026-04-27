using SevenConcentradorBridge.Native;
using SevenConcentradorBridge.Services;

if (args.Contains("--worker"))
{
    DllWorker.Run();
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient("Backend");

// Registrar serviços
builder.Services.AddSingleton<ConcentradorService>();
builder.Services.AddSingleton<PollingService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<PollingService>());

var app = builder.Build();

// Middleware de autenticação por API Key
app.Use(async (context, next) =>
{
    // Permitir health check sem auth
    if (context.Request.Path.StartsWithSegments("/api/concentrador/health"))
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

app.Run();
