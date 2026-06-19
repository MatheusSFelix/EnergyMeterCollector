
using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;

namespace EnergyMeterCollector.Api.Coletor;

public class ColetorService : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ColetorOptions _opt;
    private readonly ILogger<ColetorService> _logger;

    public ColetorService(IServiceProvider sp, ColetorOptions opt, ILogger<ColetorService> logger)
    {
        _sp = sp;
        _opt = opt;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = TimeSpan.FromSeconds(_opt.IntervaloSegundos);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var leitor = scope.ServiceProvider.GetRequiredService<IModbusLeitor>();
                var handler = scope.ServiceProvider.GetRequiredService<RegistrarLeituraHandler>();

                var l = await leitor.LerAsync(stoppingToken);
                await handler.HandleAsync(new RegistrarLeituraCommand(_opt.NumeroSerie, DateTime.UtcNow, l.TensaoVolts, l.CorrenteAmperes, l.EnergiaAtivaKwh), stoppingToken);

                _logger.LogInformation("Coletado {Serie}: {V}V", _opt.NumeroSerie, l.TensaoVolts);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha no ciclo de coleta (broker/simulador?); tenta no próximo");
            }
            try { await Task.Delay(intervalo, stoppingToken); } catch (OperationCanceledException) { break; }
        }
    }
}
