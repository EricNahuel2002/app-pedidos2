using Microsoft.EntityFrameworkCore;
using Moq;
using Usuarios.contexto;
using Usuarios.repositorio;
using Usuarios.Test.fixture;

namespace Usuarios.Test
{
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
        public async Task QueSePuedaIniciarSesion()
        {

        }

        [Fact]
        public async Task SiElContextoLanzaExcepcionElRepositorioLoPropaga()
        {

        }
        }
    }
