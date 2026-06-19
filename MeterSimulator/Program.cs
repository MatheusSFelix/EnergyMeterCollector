using FluentModbus;
using MeterSimulator;
using System.Net;

var server = new ModbusTcpServer();
server.Start(new IPEndPoint(IPAddress.Any, 502));

Console.WriteLine("MeterSimulator ouvindo em 0.0.0.0:502 (Ctrl+C para sair)");

RegistradorMap.Preencher(server, tensao: 220.5f, corrente: 4.2f, energia: 1234.7f);

await Task.Delay(Timeout.Infinite);