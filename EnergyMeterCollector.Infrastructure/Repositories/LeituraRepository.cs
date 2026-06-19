using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Domain.Entities;
using EnergyMeterCollector.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyMeterCollector.Infrastructure.Repositories;

public class LeituraRepository : ILeituraRepository
{
    private readonly AppDbContext _db;

    public LeituraRepository(AppDbContext db) => _db = db;
        

    public async Task AdicionarAsync(Leitura leitura, CancellationToken ct = default)
    {
        _db.Leituras.Add(leitura);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Leitura>> ObterPorMedidorAsync(string numeroSerie, DateTime? inicio, DateTime? fim, CancellationToken ct = default)
    {
        var query = _db.Leituras
            .Join(_db.Medidores, l => l.MedidorId, m => m.Id, (l, m) => new { l, m.NumeroSerie })
            .Where(x => x.NumeroSerie == numeroSerie);

        if (inicio is not null) query = query.Where(x => x.l.Timestamp >= inicio);
        if (fim is not null) query = query.Where(x => x.l.Timestamp <= fim);

        return await query.Select(x => x.l).OrderBy(l => l.Timestamp).ToListAsync(ct);
    }
}
