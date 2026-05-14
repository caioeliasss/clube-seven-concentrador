using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SevenConcentradorBridge.Models;

namespace SevenConcentradorBridge.Services;

public class PollingService : BackgroundService
{
    private readonly ConcentradorService _concentrador;
    private readonly ILogger<PollingService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    // Rastreia bicos que foram presetados e precisam ser monitorados
    private readonly Dictionary<string, string> _bicosMonitorados = new(); // bico -> idConcentrador

    public PollingService(
        ConcentradorService concentrador,
        ILogger<PollingService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _concentrador = concentrador;
        _logger = logger;
        _config = config;
        _httpClient = httpClientFactory.CreateClient("Backend");
    }

    public void MonitorarBico(string bico, string idConcentrador)
    {
        lock (_bicosMonitorados)
        {
            _bicosMonitorados[bico] = idConcentrador;
            _logger.LogInformation("Monitorando bico {Bico} com id {Id}", bico, idConcentrador);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = int.Parse(_config["Polling:IntervaloMs"] ?? "2000");

        // Conectar ao concentrador na inicialização
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_concentrador.Conectar()) break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao concentrador");
            }
            _logger.LogWarning("Tentando reconectar ao concentrador em 5s...");
            await Task.Delay(5000, stoppingToken);
        }

        _logger.LogInformation("Polling iniciado com intervalo de {Intervalo}ms", intervalo);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VerificarAbastecimentosFinalizados();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no polling do concentrador");
            }

            await Task.Delay(intervalo, stoppingToken);
        }

        _concentrador.Desconectar();
    }

    private async Task VerificarAbastecimentosFinalizados()
    {
        List<string> bicosParaRemover;
        lock (_bicosMonitorados)
        {
            if (_bicosMonitorados.Count == 0) return;
            bicosParaRemover = new List<string>();
        }

        // Ler abastecimento finalizado da memória do concentrador
        var dados = _concentrador.LerAbastecimento();
        if (string.IsNullOrEmpty(dados)) return;

        // Parsear dados do protocolo Companytec
        // Formato: canal(2) + total_dinheiro + total_litros + PU + tempo + data + hora + ...
        var canal = dados.Length >= 2 ? dados[..2] : "";

        lock (_bicosMonitorados)
        {
            if (!_bicosMonitorados.ContainsKey(canal)) return;
        }

        // Abastecimento finalizado neste bico — enviar webhook
        var payload = ParseAbastecimento(dados, canal);

        lock (_bicosMonitorados)
        {
            if (_bicosMonitorados.TryGetValue(canal, out var id))
            {
                payload.IdConcentrador = id;
                _bicosMonitorados.Remove(canal);
            }
        }

        _concentrador.IncrementarPonteiro();
        await EnviarWebhook(payload);
    }

    private WebhookPayload ParseAbastecimento(string dados, string canal)
    {
        // Parse básico do protocolo — ajustar conforme formato real do concentrador
        var payload = new WebhookPayload
        {
            Bico = canal,
            Status = "aguardando_pagamento"
        };

        try
        {
            // O formato exato depende do protocolo Companytec
            // Tipicamente: bico(2) + valor(10) + litros(10) + PU(6) + tempo(8) + data(10) + hora(5)
            if (dados.Length >= 22)
            {
                if (decimal.TryParse(dados[2..12].Trim(), out var dinheiro))
                    payload.TotalDinheiro = dinheiro / 100m;
                if (double.TryParse(dados[12..22].Trim(), out var litros))
                    payload.TotalLitros = litros / 1000.0;
                if (decimal.TryParse(dados[22..28].Trim(), out var pu))
                    payload.PrecoUnitario = pu / 1000m;
            }
        }
        catch (Exception)
        {
            // Se falha o parse, envia com dados zerados — backend trata
        }

        return payload;
    }

    private async Task EnviarWebhook(WebhookPayload payload)
    {
        var backendUrl = _config["Backend:WebhookUrl"]
            ?? "https://seu-backend.com/api/abastecimento/webhook-concentrador";
        var apiKey = _config["Backend:ApiKey"] ?? "";

        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, backendUrl)
            {
                Content = content
            };
            if (!string.IsNullOrEmpty(apiKey))
                request.Headers.Add("X-Api-Key", apiKey);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Webhook enviado: bico {Bico}", payload.Bico);
            else
                _logger.LogError("Webhook falhou: {Status}", response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar webhook para o backend");
        }
    }
}
