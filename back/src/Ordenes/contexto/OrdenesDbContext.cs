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
        public DbSet<Resena> Resenas { get; set; } = null!;

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

            var resenaBuilder = modelBuilder.Entity<Resena>();

            resenaBuilder.HasKey(r => r.Id);
            resenaBuilder.Property(r => r.Id).ValueGeneratedOnAdd();

            resenaBuilder.Property(r => r.IdOrden).IsRequired();
            resenaBuilder.HasIndex(r => r.IdOrden).IsUnique();

            resenaBuilder.Property(r => r.IdCliente).IsRequired();
            resenaBuilder.Property(r => r.IdRepartidor).IsRequired();
            resenaBuilder.Property(r => r.NombreCliente).IsRequired().HasMaxLength(150);
            resenaBuilder.Property(r => r.NombreRepartidor).IsRequired().HasMaxLength(150);
            resenaBuilder.Property(r => r.Puntaje).IsRequired();
            resenaBuilder.Property(r => r.Comentario).HasMaxLength(500);

            resenaBuilder.Property(r => r.FechaCreacion)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            resenaBuilder.ToTable("Resenas");
        }
    }
}
