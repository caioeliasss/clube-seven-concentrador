using System.Globalization;
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
    private readonly ConfigService _configService;
    private readonly IConfiguration _config;
    private readonly ILogger<ConcentradorController> _logger;

    public ConcentradorController(
        ConcentradorService concentrador,
        PollingService polling,
        ConfigService configService,
        IConfiguration config,
        ILogger<ConcentradorController> logger)
    {
        _concentrador = concentrador;
        _polling = polling;
        _configService = configService;
        _config = config;
        _logger = logger;
    }

    [HttpPost("preset")]
    public IActionResult Preset([FromBody] PresetRequest request)
    {
        var sucesso = _concentrador.PresetarBomba(request.Bico, request.Valor);
        if (!sucesso)
            return StatusCode(500, new { erro = "Falha ao presetar bomba" });

        return Ok(new { sucesso = true, bico = request.Bico });
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
    public async Task VisualizacaoStream(
        [FromQuery] int? intervaloMs,
        [FromQuery] string? bico,
        CancellationToken ct)
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

        string? bicoFiltro = null;
        if (!string.IsNullOrWhiteSpace(bico))
        {
            if (int.TryParse(bico, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) && n >= 1 && n <= 32)
                bicoFiltro = n.ToString("D2");
            else
            {
                Response.StatusCode = 400;
                await Response.WriteAsync($"event: erro\ndata: {{\"erro\":\"bico inválido (use 1-32)\"}}\n\n", ct);
                return;
            }
        }

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

                object body;
                if (bicoFiltro != null)
                {
                    var match = resp.Bicos.FirstOrDefault(b => b.Bico == bicoFiltro);
                    body = new { dados = resp.Raw, bico = match, ts = DateTime.UtcNow };
                }
                else
                {
                    body = new { dados = resp.Raw, bicos = resp.Bicos, ts = DateTime.UtcNow };
                }

                var payload = System.Text.Json.JsonSerializer.Serialize(body, jsonOpts);

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

    [HttpGet("abastecimento/buscar")]
    public IActionResult BuscarAbastecimento(
        [FromQuery] string? bico,
        [FromQuery] decimal? litros,
        [FromQuery] decimal toleranciaLitros = 0.5m,
        [FromQuery] DateTime? data = null,
        [FromQuery] int toleranciaMin = 5,
        [FromQuery] int maxScan = 200)
    {
        if (string.IsNullOrWhiteSpace(bico))
            return BadRequest(new { erro = "bico obrigatório" });

        try
        {
            var matches = _concentrador.BuscarAbastecimento(
                bico, litros, toleranciaLitros, data, toleranciaMin, maxScan);
            return Ok(new
            {
                total = matches.Count,
                criterios = new { bico, litros, toleranciaLitros, data, toleranciaMin, maxScan },
                matches
            });
        }
        catch (ArgumentException ex) { return BadRequest(new { erro = ex.Message }); }
        catch (InvalidOperationException ex) { return StatusCode(503, new { erro = ex.Message }); }
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

    [HttpPost("ler-incrementar")]
    public IActionResult LerIncrementar()
    {
        try
        {
            var resp = _concentrador.LerEIncrementar();
            if (resp.Vazio)
                return NotFound(new { erro = "Nenhum abastecimento na memória", raw = resp.Raw });

            return Ok(new
            {
                bico = resp.Bico,
                volume = resp.Volume,
                valorTotal = resp.ValorTotal,
                valorPorLitro = resp.ValorPorLitro,
                ts = resp.Ts,
                raw = resp.Raw
            });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { erro = ex.Message });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "ok",
            conectado = _concentrador.IsConnected,
            timestamp = DateTime.UtcNow
        });
    }

    // ===== Painel: configuração e controle de conexão =====

    [HttpGet("config")]
    public IActionResult LerConfig()
    {
        var cfg = _configService.LerConfig();

        // GET é liberado sem auth para o painel popular os padrões no primeiro acesso.
        // Sem X-Api-Key válida, mascara os segredos para não vazarem na LAN; o operador
        // só vê as chaves depois de colar a sua e recarregar.
        var apiKey = _config["Auth:ApiKey"];
        var requestKey = Request.Headers["X-Api-Key"].FirstOrDefault();
        var autenticado = string.IsNullOrEmpty(apiKey) || requestKey == apiKey;
        if (!autenticado)
        {
            cfg.AuthApiKey = null;
            cfg.BackendApiKey = null;
        }

        return Ok(cfg);
    }

    [HttpPost("config")]
    public IActionResult SalvarConfig([FromBody] ConfigDto dto)
    {
        if (dto == null)
            return BadRequest(new { erro = "Corpo vazio" });

        try
        {
            var requerRestart = _configService.SalvarConfig(dto);
            return Ok(new { sucesso = true, requerRestart });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao salvar configuração");
            return StatusCode(500, new { erro = ex.Message });
        }
    }

    [HttpPost("conectar")]
    public IActionResult Conectar()
    {
        var ok = _concentrador.Conectar();
        return ok
            ? Ok(new { conectado = true })
            : StatusCode(503, new { conectado = false, erro = "Falha ao conectar ao concentrador" });
    }

    [HttpPost("desconectar")]
    public IActionResult Desconectar()
    {
        _concentrador.Desconectar();
        return Ok(new { conectado = false });
    }

    [HttpPost("reiniciar")]
    public IActionResult Reiniciar()
    {
        var exe = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exe))
            return StatusCode(500, new { erro = "Não foi possível localizar o executável" });

        _logger.LogWarning("Reinício solicitado pelo painel — respawn de {Exe}", exe);

        // Sobe novo processo após um pequeno atraso (para o atual liberar a porta) e encerra este.
        _ = Task.Run(async () =>
        {
            await Task.Delay(800);
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = exe,
                    UseShellExecute = true,
                    WorkingDirectory = AppContext.BaseDirectory,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao respawnar processo");
            }
            Environment.Exit(0);
        });

        return Ok(new { sucesso = true, mensagem = "Reiniciando..." });
    }
}
