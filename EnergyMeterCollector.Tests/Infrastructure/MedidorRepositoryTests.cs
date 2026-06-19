using EnergyMeterCollector.Domain.Entities;
using EnergyMeterCollector.Infrastructure.Persistence;
using EnergyMeterCollector.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EnergyMeterCollector.Tests.Infrastructure;

public class MedidorRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public MedidorRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var db = new AppDbContext(_options);
        db.Database.EnsureCreated();
    }

    private AppDbContext NovoContexto() => new(_options);

    [Fact]
    public async Task Adicionar_PersisteEPodeSerLidoContextoNovo()
    {
        using (var db = NovoContexto())
        {
            var repo = new MedidorRepository(db);
            await repo.AdicionarAsync(new Medidor("MED-001"));
        }

        using (var db = NovoContexto())
        {
            var repo = new MedidorRepository(db);
            var encontrado = await repo.ObterPorNumeroSerieAsync("MED-001");

            encontrado.Should().NotBeNull();
            encontrado!.NumeroSerie.Should().Be("MED-001");
        }
    }

    public void Dispose() => _connection.Dispose();
}
