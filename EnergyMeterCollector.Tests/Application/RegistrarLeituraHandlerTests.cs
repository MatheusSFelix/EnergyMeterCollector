using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;
using EnergyMeterCollector.Domain.Entities;
using FluentAssertions;
using Moq;

namespace EnergyMeterCollector.Tests.Application;

public class RegistrarLeituraHandlerTests
{
    private readonly Mock<IMedidorRepository> _medidorRepo = new();
    private readonly Mock<ILeituraRepository> _leituraRepo = new();
    private readonly Mock<ILeituraPublisher> _leituraPublisher = new();
    private static RegistrarLeituraCommand CommandValido() => new("MED-001", DateTime.UtcNow, 220m, 5m, 1.5m);

    [Fact]
    public async Task QuandoMedidorNaoExiste_CriaMedidorERegistraLeitura()
    {
        // Arrange
        _medidorRepo.Setup(r => r.ObterPorNumeroSerieAsync("MED-001", It.IsAny<CancellationToken>())).ReturnsAsync((Medidor?)null);

        var handler = new RegistrarLeituraHandler(_medidorRepo.Object, _leituraRepo.Object, _leituraPublisher.Object);

        var id = await handler.HandleAsync(CommandValido());

        id.Should().NotBeEmpty();
        _medidorRepo.Verify(r => r.AdicionarAsync(It.IsAny<Medidor>(), It.IsAny<CancellationToken>()), Times.Once);
        _leituraRepo.Verify(r => r.AdicionarAsync(It.IsAny<Leitura>(), It.IsAny<CancellationToken>()), Times.Once);
        _leituraPublisher.Verify(r => r.PublicarAsync("MED-001", It.IsAny<LeituraInstantanea>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task QuandoMedidorJaExiste_NaoCriaMedidor_ApenasRegistraLeitura()
    {
        _medidorRepo.Setup(v => v.ObterPorNumeroSerieAsync("MED-001", It.IsAny<CancellationToken>())).ReturnsAsync(new Medidor("MED-001"));

        var handler = new RegistrarLeituraHandler(_medidorRepo.Object, _leituraRepo.Object, _leituraPublisher.Object);

        await handler.HandleAsync(CommandValido());

        _medidorRepo.Verify(v => v.AdicionarAsync(It.IsAny<Medidor>(), It.IsAny<CancellationToken>()), Times.Never);
        _leituraRepo.Verify(v => v.AdicionarAsync(It.IsAny<Leitura>(), It.IsAny<CancellationToken>()), Times.Once);
        _leituraPublisher.Verify(r => r.PublicarAsync("MED-001", It.IsAny<LeituraInstantanea>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
