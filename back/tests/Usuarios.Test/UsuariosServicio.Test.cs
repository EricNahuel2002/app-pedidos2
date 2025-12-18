using Moq;
using Usuarios.dto;
using Usuarios.entidad;
using Usuarios.excepciones;
using Usuarios.repositorio;
using Usuarios.servicio;
using Usuarios.Test.fixture;

namespace Usuarios.Test;

public class UsuariosServicioTest: IClassFixture<UsuariosServicioFixture>
{
    private Mock<IUsuariosRepositorio> _repoMock;
    private UsuariosServicio _usuarioServicio;

    public UsuariosServicioTest(UsuariosServicioFixture fixture)
    {
        _repoMock = fixture.repoMock;
        _usuarioServicio = fixture.usuarioServicio;
    }


    [Fact]
    public async Task QueSePuedaValidarCredencialesDeUsuario()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");
        Rol rol = new Rol() { Id = 1, Nombre = "cliente" };
        List<UsuarioRol> urList = new List<UsuarioRol> { new UsuarioRol { Id = 1, Rol = rol} };
        Usuario usuario = new Usuario
        {
            Id = 1, Email = dto.Email, Contrasenia = dto.Contrasenia,
            Nombre = "pepe", Cliente = null, UsuarioRoles = urList, Repartidor = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        UsuarioDto resultado = await _usuarioServicio.ValidarCredencialesDeUsuario(dto);

        Assert.Equal(dto.Email,resultado.Email);
    }

    [Fact]
    public async Task SiAlValidarCredencialesDeUsuarioSonIncorrectasElServicioLanzaCredencialesInvalidasException()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "saraza");
        Rol rol = new Rol() { Id = 1, Nombre = "cliente" };
        List<UsuarioRol> urList = new List<UsuarioRol> { new UsuarioRol { Id = 1, Rol = rol } };
        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = dto.Email,
            Contrasenia = "123",
            Nombre = "pepe",
            Cliente = null,
            UsuarioRoles = urList,
            Repartidor = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<CredencialesInvalidasException>(async () => await _usuarioServicio.ValidarCredencialesDeUsuario(dto));

    }

    [Fact]
    public async Task SiAlValidarCredencialesDeUsuarioElUsuarioRolesAsignadoNoExisteElServicioLanzaInvalidOperationException()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");
        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = dto.Email,
            Contrasenia = dto.Contrasenia,
            Nombre = "pepe",
            Cliente = null,
            UsuarioRoles = null,
            Repartidor = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _usuarioServicio.ValidarCredencialesDeUsuario(dto));
    }

    [Fact]
    public async Task SiAlValidarCredencialesDeUsuarioElRolsAsignadoNoExisteElServicioLanzaInvalidOperationException()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");
        List<UsuarioRol> urList = new List<UsuarioRol> { new UsuarioRol { Id = 1, Rol = null } };
        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = dto.Email,
            Contrasenia = dto.Contrasenia,
            Nombre = "pepe",
            Cliente = null,
            UsuarioRoles = urList,
            Repartidor = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _usuarioServicio.ValidarCredencialesDeUsuario(dto));
    }


    [Fact]
    public async Task QueSePuedaObtenerUsuarioCliente()
    {
        Cliente cliente = new Cliente { IdUsuario = 1, Usuario = null, Direccion = "ldfs", NumeroTelefonico = "215", Saldo = 1 };
        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = "saraza@saraza.com",
            Contrasenia = "123",
            Nombre = "pepe",
            Repartidor = null,
            Cliente = cliente,
            UsuarioRoles = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorId(1)).ReturnsAsync(usuario);

        var usuarioObtenido = await _usuarioServicio.ObtenerUsuarioCliente(1);

        Assert.NotNull(usuarioObtenido);
        Assert.IsType<UsuarioClienteDto>(usuarioObtenido);
        Assert.Equal(usuario.Email, usuarioObtenido.Email);
    }

    [Fact]
    public async Task SiAlObtenerUsuarioClienteElIdNoExisteElServicioLanzaKeyNotFoundException()
    {
        Cliente cliente = new Cliente { IdUsuario = 1, Usuario = null, Direccion = "ldfs", NumeroTelefonico = "215", Saldo = 1 };
        Usuario usuario = null;

        _repoMock.Setup(r => r.ObtenerUsuarioPorId(1)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _usuarioServicio.ObtenerUsuarioCliente(1));
    }

    [Fact]
    public async Task SiAlObtenerUsuarioClienteElClienteEsNuloElServicioLanzaInvalidOperationException()
    {
        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = "saraza@saraza.com",
            Contrasenia = "123",
            Nombre = "pepe",
            Repartidor = null,
            Cliente = null,
            UsuarioRoles = null
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorId(1)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _usuarioServicio.ObtenerUsuarioCliente(1));
    }

}
