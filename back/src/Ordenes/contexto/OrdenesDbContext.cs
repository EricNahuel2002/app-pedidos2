using Microsoft.EntityFrameworkCore;
using Ordenes.Entidad;

namespace Ordenes.contexto;

public class OrdenesDbContext: DbContext
{
    public OrdenesDbContext(DbContextOptions<OrdenesDbContext> options)
            : base(options)
    {
    }

    public DbSet<Orden> Ordenes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orden>()
            .Property(o => o.FechaOrden)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        base.OnModelCreating(modelBuilder);
    }
}
