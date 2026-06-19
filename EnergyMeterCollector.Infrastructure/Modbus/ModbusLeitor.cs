using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;
using FluentModbus;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace EnergyMeterCollector.Infrastructure.Modbus;

public class ModbusLeitor : IModbusLeitor
{
    private readonly ModbusOptions _opt;

    public ModbusLeitor(ModbusOptions opt) => _opt = opt;

    public Task<LeituraInstantanea> LerAsync(CancellationToken ct = default)
    {
        using var client = new ModbusTcpClient();
        var ip = IPAddress.TryParse(_opt.Host, out var parsed) 
            ? parsed 
            : Dns.GetHostAddresses(_opt.Host).First(a => a.AddressFamily == AddressFamily.InterNetwork);
        client.Connect(new IPEndPoint(ip, _opt.Porta));

        var dados = client.ReadHoldingRegisters<float>(_opt.UnitId, _opt.EnderecoBase, 3);

        var leitura = new LeituraInstantanea(
            (decimal)dados[0],
            (decimal)dados[1],
            (decimal)dados[2]);

        return Task.FromResult(leitura);
    }
}
