using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterSimulator;

public static class RegistradorMap
{
    public static void Preencher(ModbusTcpServer server, float tensao, float corrente, float energia)
    {
        var registers = server.GetHoldingRegisterBuffer<float>();
        registers[0] = tensao;
        registers[1] = corrente;
        registers[2] = energia;
    }
}
