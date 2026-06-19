namespace EnergyMeterCollector.Application.Leituras;

public record LeituraInstantanea(decimal TensaoVolts, decimal CorrenteAmperes, decimal EnergiaAtivaKwh);