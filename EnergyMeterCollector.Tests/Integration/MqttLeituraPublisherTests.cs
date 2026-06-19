using EnergyMeterCollector.Application.Leituras;
using EnergyMeterCollector.Infrastructure.Messaging;
using FluentAssertions;
using MQTTnet;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnergyMeterCollector.Tests.Integration;

public class MqttLeituraPublisherTests
{
    [Fact(Skip = "Requer broker MQTT (Mosquitto) em 127.0.0.1:1883 — habilitar quando o Docker Compose subir (passo 11)")]
    public async Task PublicarAsync_envia_leitura_no_topico_do_medidor()
    {
        var opt = new MqttOptions { Host = "127.0.0.1", Porta = 1883 };
        var factory = new MqttClientFactory();

        using var assinante = factory.CreateMqttClient();
        var recebida = new TaskCompletionSource<string>();
        assinante.ApplicationMessageReceivedAsync += e =>
        {
            recebida.TrySetResult(Encoding.UTF8.GetString(e.ApplicationMessage.Payload.ToArray()));
            return Task.CompletedTask;
        };

        await assinante.ConnectAsync(new MqttClientOptionsBuilder().WithTcpServer(opt.Host, opt.Porta).Build());
        await assinante.SubscribeAsync("medidores/MED-001/leituras");

        var publisher = new MqttLeituraPublisher(opt);
        await publisher.PublicarAsync("MED-001", new LeituraInstantanea(220.5m, 4.2m, 1234.7m));

        var json = await recebida.Task.WaitAsync(TimeSpan.FromSeconds(5));
        var leitura = JsonSerializer.Deserialize<LeituraInstantanea>(json);

        leitura.Should().NotBeNull();
        leitura!.TensaoVolts.Should().Be(220.5m);
    }
}
