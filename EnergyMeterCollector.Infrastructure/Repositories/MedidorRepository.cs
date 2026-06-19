using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Domain.Entities;
using EnergyMeterCollector.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyMeterCollector.Infrastructure.Repositories;

public class MedidorRepository : IMedidorRepository
{
    private readonly AppDbContext _db;

    public MedidorRepository(AppDbContext db) => _db = db;
    

    public async Task AdicionarAsync(Medidor medidor, CancellationToken ct = default)
    {
        _db.Medidores.Add(medidor);
        await _db.SaveChangesAsync(ct);
    }

    public Task<Medidor?> ObterPorNumeroSerieAsync(string numeroSerie, CancellationToken ct = default) => _db.Medidores.FirstOrDefaultAsync(m => m.NumeroSerie == numeroSerie, ct);
}
