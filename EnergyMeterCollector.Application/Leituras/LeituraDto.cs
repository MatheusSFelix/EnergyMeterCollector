namespace EnergyMeterCollector.Application.Leituras;

public record LeituraDto(DateTime Timestamp, decimal TensaoVolts, decimal CorrenteAmperes, decimal EnergiaAtivaKwh);