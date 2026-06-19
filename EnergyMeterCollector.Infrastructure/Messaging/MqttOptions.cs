namespace EnergyMeterCollector.Infrastructure.Messaging;

public class MqttOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Porta { get; set; } = 1883;
    public string TopicoBase { get; set; } = "medidores";
}
