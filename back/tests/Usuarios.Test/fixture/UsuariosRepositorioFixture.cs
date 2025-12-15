using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.contexto;
using Usuarios.repositorio;

namespace Usuarios.Test.fixture;

public class UsuariosRepositorioFixture
{
    public DbContextOptions<UsuariosDbContext> _ctx;
    public IUsuariosRepositorio usuarioRepo;

    public UsuariosRepositorioFixture()
    {
        _ctx = new DbContextOptionsBuilder<UsuariosDbContext>()
        .UseInMemoryDatabase(databaseName: "usuario_db" + Guid.NewGuid().ToString())
        .Options;
        usuarioRepo = new UsuariosRepositorio(new UsuariosDbContext(_ctx));
    }
}
