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
        var dados = _concentrador.LerAbastecimentoPAF();
        return Ok(new { dados });
    }

    [HttpGet("visualizacao")]
    public IActionResult Visualizacao()
    {
        var dados = _concentrador.LerVisualizacao();
        return Ok(new { dados });
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
    public IActionResult PrecosDll([FromQuery] int niveis = 2)
    {
        if (niveis < 0 || niveis > 2)
            return BadRequest(new { erro = "niveis deve ser 0, 1 ou 2" });

        try
        {
            var precos = _concentrador.LerPrecosDll(niveis);
            return Ok(precos);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("precos-dll/{bico}")]
    public IActionResult PrecoDllPorBico(string bico, [FromQuery] int niveis = 2)
    {
        if (niveis < 0 || niveis > 2)
            return BadRequest(new { erro = "niveis deve ser 0, 1 ou 2" });

        try
        {
            var preco = _concentrador.LerPrecoDllPorBico(bico, niveis);
            if (preco == null)
                return NotFound(new { erro = $"Bico {bico} não encontrado ou DLL retornou falha" });

            return Ok(preco);
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
