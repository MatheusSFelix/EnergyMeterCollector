namespace EnergyMeterCollector.Application.Leituras;

public record RegistrarLeituraCommand(
    string NumeroSerie,
    DateTime Timestamp,
    decimal TensaoVolts,
    decimal CorrentAmperes,
    decimal EnergiaAtivaKwh
);
