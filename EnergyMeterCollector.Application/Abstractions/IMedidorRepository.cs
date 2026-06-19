using EnergyMeterCollector.Domain.Entities;

namespace EnergyMeterCollector.Application.Abstractions;

public interface IMedidorRepository
{
    Task<Medidor?> ObterPorNumeroSerieAsync(string numeroSerie, CancellationToken ct = default);
    Task AdicionarAsync(Medidor medidor, CancellationToken ct  = default);
}
