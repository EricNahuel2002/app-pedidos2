using Microsoft.EntityFrameworkCore;
using Ordenes.Entidad;

namespace Ordenes.contexto
{
    public class OrdenesDbContext : DbContext
    {
        public OrdenesDbContext(DbContextOptions<OrdenesDbContext> options) : base(options)
        {
        }

        public DbSet<Orden> Ordenes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Orden>();

            builder.HasKey(o => o.IdOrden);
            builder.Property(o => o.IdOrden).ValueGeneratedOnAdd();

            builder.Property(o => o.IdCliente).IsRequired();
            builder.Property(o => o.IdMenu).IsRequired();

            builder.Property(o => o.NombreMenu).IsRequired();
            builder.Property(o => o.NombreCliente).IsRequired().HasMaxLength(150);
            builder.Property(o => o.EmailCliente).IsRequired().HasMaxLength(255);
            builder.Property(o => o.PrecioAPagar).IsRequired();
            builder.Property(o => o.Estado)
                       .IsRequired()
                       .HasMaxLength(50)
                       .HasDefaultValue("PENDIENTE");
            builder.Property(o => o.Direccion).IsRequired().HasMaxLength(500);

            builder.Property(o => o.IdRepartidor).IsRequired(false);
            builder.Property(o => o.NombreRepartidor).IsRequired(false).HasMaxLength(150);
            builder.Property(o => o.DniRepartidor).IsRequired(false).HasMaxLength(50);

            builder.Property(o => o.FechaOrden)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.ToTable("Ordenes");
        }
    }
}
