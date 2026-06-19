using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Domain.Entities;

namespace EnergyMeterCollector.Application.Leituras;

public class RegistrarLeituraHandler
{
    private readonly IMedidorRepository _medidorRepository;
    private readonly ILeituraRepository _leituraRepository;
    private readonly ILeituraPublisher _leituraPublisher;

    public RegistrarLeituraHandler(IMedidorRepository medidorRepository, ILeituraRepository leituraRepository, ILeituraPublisher leituraPublisher)
    {
        _medidorRepository = medidorRepository;
        _leituraRepository = leituraRepository;
        _leituraPublisher = leituraPublisher;
    }

    public async Task<Guid> HandleAsync(RegistrarLeituraCommand cmd, CancellationToken ct = default)
    {
        var medidor = await _medidorRepository.ObterPorNumeroSerieAsync(cmd.NumeroSerie, ct);

        if (medidor is null)
        {
            medidor = new Medidor(cmd.NumeroSerie);
            await _medidorRepository.AdicionarAsync(medidor, ct);
        }

        var leitura = medidor.RegistrarLeitura(cmd.Timestamp, cmd.TensaoVolts, cmd.CorrentAmperes, cmd.EnergiaAtivaKwh);
        await _leituraRepository.AdicionarAsync(leitura, ct);

        await _leituraPublisher.PublicarAsync(cmd.NumeroSerie, new LeituraInstantanea(cmd.TensaoVolts, cmd.CorrentAmperes, cmd.EnergiaAtivaKwh), ct);

        return leitura.Id;
    }
}
