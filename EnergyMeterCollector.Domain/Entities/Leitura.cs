namespace EnergyMeterCollector.Domain.Entities;

public class Leitura
{
    public Guid Id { get; private set; }
    public Guid MedidorId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public decimal TensaoVolts { get; private set; }
    public decimal CorrenteAmperes { get; private set; }
    public decimal EnergiaAtivaKwh { get; private set; }

    public Leitura(Guid medidorId, DateTime timestamp, decimal tensaoVolts, decimal correnteAmperes, decimal energiaAtivaKwh)
    {
        if (tensaoVolts < 0)
            throw new ArgumentException("Tensão em volts não pode ser negativa.");

        if (correnteAmperes < 0)
            throw new ArgumentException("Corrente em amperes não pode ser negativa.");

        if (energiaAtivaKwh < 0)
            throw new ArgumentException("Energia Ativa não pode ser negativa.");

        if (timestamp == default)
            throw new ArgumentException("Data/hora inválido.");

        TensaoVolts = tensaoVolts;
        CorrenteAmperes = correnteAmperes;
        EnergiaAtivaKwh = energiaAtivaKwh;
        Timestamp = timestamp;
        MedidorId = medidorId;
        Id = Guid.NewGuid();
    }

}
