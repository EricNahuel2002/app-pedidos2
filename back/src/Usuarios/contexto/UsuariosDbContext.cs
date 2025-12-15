using Microsoft.EntityFrameworkCore;
using Usuarios.entidad;

namespace Usuarios.contexto;

public class UsuariosDbContext: DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
            : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Repartidor> Repartidores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
            .HasKey(c => c.IdUsuario);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Usuario)
            .WithOne(u => u.Cliente)
            .HasForeignKey<Cliente>(c => c.IdUsuario);

        modelBuilder.Entity<Repartidor>()
        .HasKey(r => r.IdUsuario);

        modelBuilder.Entity<Repartidor>()
            .HasOne(r => r.Usuario)
            .WithOne(u => u.Repartidor)
            .HasForeignKey<Repartidor>(r => r.IdUsuario);

        modelBuilder.Entity<UsuarioRol>()
        .HasKey(ur => ur.Id);

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Usuario)
            .WithMany(u => u.UsuarioRoles)
            .HasForeignKey(ur => ur.IdUsuario);

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Rol)
            .WithMany(r => r.UsuarioRoles)
            .HasForeignKey(ur => ur.IdRol);


        base.OnModelCreating(modelBuilder);
    }
}
