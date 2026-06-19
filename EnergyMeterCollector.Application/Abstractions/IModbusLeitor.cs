using EnergyMeterCollector.Application.Leituras;
using EnergyMeterCollector.Domain.Entities;

namespace EnergyMeterCollector.Application.Abstractions;

public interface IModbusLeitor
{
    Task<LeituraInstantanea> LerAsync(CancellationToken ct = default);
}
