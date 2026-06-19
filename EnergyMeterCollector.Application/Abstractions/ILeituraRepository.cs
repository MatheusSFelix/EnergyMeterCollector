using EnergyMeterCollector.Domain.Entities;

namespace EnergyMeterCollector.Application.Abstractions;

public interface ILeituraRepository
{
    Task AdicionarAsync(Leitura leitura, CancellationToken ct = default);
    Task<IReadOnlyList<Leitura>> ObterPorMedidorAsync(string numeroSerie, DateTime? inicio, DateTime? fim, CancellationToken ct = default);
}
