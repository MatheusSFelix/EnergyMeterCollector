using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;
using MQTTnet;
using System.Text.Json;

namespace EnergyMeterCollector.Infrastructure.Messaging;

public class MqttLeituraPublisher : ILeituraPublisher
{
    private readonly MqttOptions _opt;

    public MqttLeituraPublisher(MqttOptions opt)
    {
        _opt = opt;
    }

    public async Task PublicarAsync(string numeroSerie, LeituraInstantanea leitura, CancellationToken ct = default)
    {
        var factory = new MqttClientFactory();
        using var client = factory.CreateMqttClient();

        var opcoes = new MqttClientOptionsBuilder()
            .WithTcpServer(_opt.Host, _opt.Porta)
            .Build();

        await client.ConnectAsync(opcoes, ct);

        var mensagem = new MqttApplicationMessageBuilder()
            .WithTopic($"{_opt.TopicoBase}/{numeroSerie}/leituras")
            .WithPayload(JsonSerializer.Serialize(leitura))
            .Build();

        await client.PublishAsync(mensagem, ct);
        await client.DisconnectAsync(cancellationToken: ct);
    }
}
