using EnergyMeterCollector.Domain.Entities;
using FluentAssertions;

namespace EnergyMeterCollector.Tests.Entities;

public class LeituraTests
{
    [Fact]
    public void Construtor_ComValoresValidos_CriaLeitura()
    {
        //Arrange
        var medidorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        // Act
        var leitura = new Leitura(medidorId, timestamp, 220m, 5m, 1.5m);

        // Assert
        leitura.MedidorId.Should().Be(medidorId);
        leitura.TensaoVolts.Should().Be(220m);
        leitura.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Tensao_Negativa_LancaExcecao()
    {
        // Arrange
        var medidorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        // Act
        Action act = () => new Leitura(medidorId, timestamp, -100m, 5, 1.5m);

        // Assert
        act.Should().Throw<ArgumentException>();
    }


    [Fact]
    public void Corrente_Negativa_LancaExcecao()
    {
        var medidorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        Action act = () => new Leitura(medidorId, timestamp, 220m, -5, 1.5m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Energia_Negativa_LancaExcecao()
    {
        var medidorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        Action act = () => new Leitura(medidorId, timestamp, 220m, 5, -50m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Timestamp_Default_LancaExcecao()
    {
        var medidorId = Guid.NewGuid();
        Action act = () => new Leitura(medidorId, default, 220m, 5, 1.5m);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Valores_Zero_NaoLancamExcecao()
    {
        var medidorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        Action act = () => new Leitura(medidorId, timestamp, 0, 0, 0);

        act.Should().NotThrow();
    }
}
 
