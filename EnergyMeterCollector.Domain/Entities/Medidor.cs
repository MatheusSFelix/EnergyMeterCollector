namespace EnergyMeterCollector.Domain.Entities;

public class Medidor
{
    public Guid Id { get; private set; }
    public string NumeroSerie { get; private set; }

    public Medidor(string numeroSerie)
    {
        if (string.IsNullOrWhiteSpace(numeroSerie))
            throw new ArgumentException("Número de série é obrigatório.", nameof(numeroSerie));
        NumeroSerie = numeroSerie;
        Id = Guid.NewGuid();
    }

    public Leitura RegistrarLeitura(DateTime timestamp, decimal tensaoVolts, decimal correnteAmperes, decimal energiaAtivaKwh)
    {
        return new Leitura(Id, timestamp, tensaoVolts, correnteAmperes, energiaAtivaKwh);
    }
}
