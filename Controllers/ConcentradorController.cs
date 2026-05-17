using Microsoft.AspNetCore.Mvc;
using SevenConcentradorBridge.Models;
using SevenConcentradorBridge.Services;

namespace SevenConcentradorBridge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcentradorController : ControllerBase
{
    private readonly ConcentradorService _concentrador;
    private readonly PollingService _polling;
    private readonly ILogger<ConcentradorController> _logger;

    public ConcentradorController(
        ConcentradorService concentrador,
        PollingService polling,
        ILogger<ConcentradorController> logger)
    {
        _concentrador = concentrador;
        _polling = polling;
        _logger = logger;
    }

    [HttpPost("preset")]
    public IActionResult Preset([FromBody] PresetRequest request)
    {
        var sucesso = _concentrador.PresetarBomba(request.Bico, request.Valor);
        if (!sucesso)
            return StatusCode(500, new { erro = "Falha ao presetar bomba" });

        // Gerar ID único para rastrear essa operação
        var idConcentrador = Guid.NewGuid().ToString("N")[..12];
        _polling.MonitorarBico(request.Bico, idConcentrador);

        return Ok(new { sucesso = true, idConcentrador, bico = request.Bico });
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        var status = _concentrador.LerStatus();
        return Ok(new { status });
    }

    [HttpGet("abastecimento")]
    public IActionResult LerAbastecimento()
    {
        var dados = _concentrador.LerAbastecimento();
        return Ok(new { dados });
    }

    [HttpGet("visualizacao")]
    public IActionResult Visualizacao()
    {
        var resp = _concentrador.LerVisualizacaoParsed();
        return Ok(new { dados = resp.Raw, bicos = resp.Bicos });
    }

    [HttpGet("visualizacao/stream")]
    public async Task VisualizacaoStream([FromQuery] int? intervaloMs, CancellationToken ct)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("X-Accel-Buffering", "no");
        Response.Headers.Append("Connection", "keep-alive");

        var cfgMs = int.TryParse(HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["Polling:IntervaloMs"], out var v) ? v : 1000;
        var delay = Math.Max(200, intervaloMs ?? cfgMs);

        var jsonOpts = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        };

        try
        {
            while (!ct.IsCancellationRequested)
            {
                VisualizacaoResponse resp;
                try
                {
                    resp = _concentrador.LerVisualizacaoParsed();
                }
                catch (Exception ex)
                {
                    var errJson = System.Text.Json.JsonSerializer.Serialize(new { erro = ex.Message }, jsonOpts);
                    await Response.WriteAsync($"event: erro\ndata: {errJson}\n\n", ct);
                    await Response.Body.FlushAsync(ct);
                    await Task.Delay(delay, ct);
                    continue;
                }

                var payload = System.Text.Json.JsonSerializer.Serialize(
                    new { dados = resp.Raw, bicos = resp.Bicos, ts = DateTime.UtcNow },
                    jsonOpts);

                await Response.WriteAsync($"event: visualizacao\ndata: {payload}\n\n", ct);
                await Response.Body.FlushAsync(ct);

                await Task.Delay(delay, ct);
            }
        }
        catch (OperationCanceledException) { /* cliente desconectou */ }
    }

    [HttpPost("liberar")]
    public IActionResult Liberar([FromBody] BicoRequest request)
    {
        var sucesso = _concentrador.LiberarBico(request.Bico);
        return sucesso
            ? Ok(new { sucesso = true })
            : StatusCode(500, new { erro = "Falha ao liberar bico" });
    }

    [HttpPost("bloquear")]
    public IActionResult Bloquear([FromBody] BicoRequest request)
    {
        var sucesso = _concentrador.BloquearBico(request.Bico);
        return sucesso
            ? Ok(new { sucesso = true })
            : StatusCode(500, new { erro = "Falha ao bloquear bico" });
    }

    [HttpPost("autorizar")]
    public IActionResult Autorizar([FromBody] BicoRequest request)
    {
        var sucesso = _concentrador.AutorizarBico(request.Bico);
        return sucesso
            ? Ok(new { sucesso = true })
            : StatusCode(500, new { erro = "Falha ao autorizar bico" });
    }

    [HttpPost("preco")]
    public IActionResult AlterarPreco([FromBody] SetPrecoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Bico) || string.IsNullOrWhiteSpace(request?.Preco))
            return BadRequest(new { erro = "Bico e preço obrigatórios" });

        try
        {
            var sucesso = _concentrador.AlterarPreco(request.Bico, request.Preco);
            return sucesso
                ? Ok(new { sucesso = true, bico = request.Bico, preco = request.Preco })
                : StatusCode(500, new { erro = "Falha ao alterar preço" });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("precos")]
    public IActionResult Precos()
    {
        var precos = _concentrador.LerPrecosTodos();
        return Ok(precos);
    }

    [HttpGet("precos/{bico:int}")]
    public IActionResult PrecoPorBico(string bico)
    {
        var preco = _concentrador.LerPrecoPorBico(bico);
        if (preco == null)
            return NotFound(new { erro = $"Bico {bico} não encontrado ou sem preço disponível" });
        return Ok(preco);
    }

    [HttpGet("precos-dll")]
    public IActionResult PrecosDll()
    {
        try
        {
            var precos = _concentrador.LerPrecosDll();
            return Ok(precos);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("precos-dll/{bico}")]
    public IActionResult PrecoDllPorBico(string bico)
    {
        try
        {
            var preco = _concentrador.LerPrecoDllPorBico(bico);
            if (preco == null)
                return NotFound(new { erro = $"Bico {bico} não encontrado ou DLL retornou falha" });

            return Ok(preco);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    // [HttpGet("preco-raw/{bico}")]
    // public IActionResult PrecoRaw(string bico)
    // {
    //     try
    //     {
    //         var (comando, resposta) = _concentrador.LerPrecoUnitarioRaw(bico);
    //         return Ok(new { bico, comandoEnviado = comando, respostaRaw = resposta });
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         return StatusCode(503, new { erro = ex.Message });
    //     }
    // }

    [HttpPost("native")]
    public IActionResult ComandoNativo([FromBody] NativeCommandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Comando))
            return BadRequest(new { erro = "Comando vazio" });

        try
        {
            var (comando, resposta) = _concentrador.EnviarNativo(request.Comando);
            return Ok(new { comando, resposta });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("ponteiros")]
    public IActionResult Ponteiros()
    {
        try
        {
            var p = _concentrador.LerPonteiros();
            return Ok(p);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("registro/{posicao:int}")]
    public IActionResult Registro(int posicao)
    {
        try
        {
            var reg = _concentrador.LerRegistro(posicao);
            return Ok(reg);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
    }
}
