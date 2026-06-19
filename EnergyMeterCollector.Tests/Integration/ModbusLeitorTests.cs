using EnergyMeterCollector.Infrastructure.Modbus;
using FluentAssertions;
using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMeterCollector.Tests.Integration;

public class ModbusLeitorTests
{
    [Fact]
    public async Task LerAsync_le_os_3_floats_do_servidor()
    {
        using var server = new ModbusTcpServer();
        server.Start(new IPEndPoint(IPAddress.Loopback, 1502));
        PreencherRegistradores(server);

        var leitor = new ModbusLeitor(new ModbusOptions { Host = "127.0.0.1", Porta = 1502 });
        var leitura = await leitor.LerAsync();

        leitura.TensaoVolts.Should().BeApproximately(220.5m, 0.01m);
        leitura.CorrenteAmperes.Should().BeApproximately(4.2m, 0.01m);
        leitura.EnergiaAtivaKwh.Should().BeApproximately(1234.7m, 0.01m);
    }

    private void PreencherRegistradores(ModbusTcpServer server)
    {
        var buffer = server.GetHoldingRegisterBuffer<float>();
        buffer[0] = 220.5f; buffer[1] = 4.2f; buffer[2] = 1234.7f;
    }
}
