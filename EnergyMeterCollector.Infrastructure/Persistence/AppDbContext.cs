using EnergyMeterCollector.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyMeterCollector.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

    public DbSet<Medidor> Medidores => Set<Medidor>();
    public DbSet<Leitura> Leituras => Set<Leitura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medidor>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.NumeroSerie).IsRequired().HasMaxLength(100);
            e.HasIndex(m => m.NumeroSerie).IsUnique();
        });

        modelBuilder.Entity<Leitura>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.TensaoVolts).HasPrecision(18, 3);
            e.Property(l => l.CorrenteAmperes).HasPrecision(18, 3);
            e.Property(l => l.EnergiaAtivaKwh).HasPrecision(18, 3);
            e.HasIndex(l => new { l.MedidorId, l.Timestamp });
            e.HasOne<Medidor>()
                .WithMany()
                .HasForeignKey(l => l.MedidorId);
        });
    }
}
