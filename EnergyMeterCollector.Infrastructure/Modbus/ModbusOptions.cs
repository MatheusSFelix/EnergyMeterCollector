namespace EnergyMeterCollector.Infrastructure.Modbus;

public class ModbusOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Porta { get; set; } = 502;
    public byte UnitId { get; set; } = 0;
    public ushort EnderecoBase { get; set; } = 0;
}
