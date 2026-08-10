using Microsoft.EntityFrameworkCore;
using Notificaciones.entidad;

namespace Notificaciones.contexto;

public class NotificacionesDbContext : DbContext
{
    public NotificacionesDbContext(DbContextOptions<NotificacionesDbContext> options) : base(options)
    {
    }

    public DbSet<Notificacion> Notificaciones { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<Notificacion>();

        builder.HasKey(n => n.IdNotificacion);
        builder.Property(n => n.IdNotificacion).ValueGeneratedOnAdd();

        builder.Property(n => n.IdUsuario).IsRequired();
        builder.Property(n => n.Mensaje).IsRequired().HasMaxLength(500);
        builder.Property(n => n.Leida).IsRequired().HasDefaultValue(false);

        builder.Property(n => n.FechaCreacion)
               .HasColumnType("datetime")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.ToTable("Notificaciones");
    }
}
