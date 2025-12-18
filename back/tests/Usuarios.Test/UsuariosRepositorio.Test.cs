using Microsoft.EntityFrameworkCore;
using Moq;
using Usuarios.contexto;
using Usuarios.entidad;
using Usuarios.repositorio;
using Usuarios.Test.fixture;

namespace Usuarios.Test;

public class UsuariosRepositorioTest: IClassFixture<UsuariosRepositorioFixture>
{

    private DbContextOptions<UsuariosDbContext> _ctx;
    private IUsuariosRepositorio _repo;

    public UsuariosRepositorioTest(UsuariosRepositorioFixture fixture)
    {
        _ctx = fixture._ctx;
        _repo = fixture.usuarioRepo;
    }

    [Fact]
    public async Task QueSePuedaObtenerUsuarioPorEmail()
    {
        using (var ctx = new UsuariosDbContext(_ctx)) 
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Usuario usuario = new Usuario { Cliente = null, Email = "saraza@saraza.com",
                Contrasenia = "svxc", Id = 1, Nombre = "BVbv", Repartidor = null, UsuarioRoles = null };

            ctx.Usuarios.Add(usuario);

            await ctx.SaveChangesAsync();
        }
        using (var ctx = new UsuariosDbContext(_ctx))
        {
            string email = "saraza@saraza.com";
            Usuario? usuario = await ctx.Usuarios.Where(u => u.Email.Equals(email))
            .Include(u => u.UsuarioRoles)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync();

            Assert.NotNull(usuario);
            Assert.Equal(email, usuario.Email);
        }
    }

    [Fact]
    public async Task QueSePuedaObtenerUsuarioPorId()
    {
        using (var ctx = new UsuariosDbContext(_ctx))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Usuario usuario = new Usuario
            {
                Cliente = null,
                Email = "saraza@saraza.com",
                Contrasenia = "svxc",
                Id = 1,
                Nombre = "BVbv",
                Repartidor = null,
                UsuarioRoles = null
            };

            ctx.Usuarios.Add(usuario);

            await ctx.SaveChangesAsync();
        }
        using (var ctx = new UsuariosDbContext(_ctx))
        {
            int id = 1;
            Usuario? usuario = await ctx.Usuarios.Where(u => u.Id == id)
            .Include(u => u.Cliente)
            .FirstOrDefaultAsync();

            Assert.NotNull(usuario);
            Assert.Equal(id, usuario.Id);
        }
    }
}

