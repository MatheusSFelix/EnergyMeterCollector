using EnergyMeterCollector.Application.Leituras;

namespace EnergyMeterCollector.Application.Abstractions;

public interface ILeituraPublisher
{
    Task PublicarAsync(string numeroSerie, LeituraInstantanea leitura, CancellationToken ct = default);
}
