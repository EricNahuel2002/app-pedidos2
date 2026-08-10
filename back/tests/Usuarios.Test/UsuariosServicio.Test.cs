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
            Id = 1, Email = dto.Email, Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.Contrasenia),
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
            Contrasenia = BCrypt.Net.BCrypt.HashPassword("123"),
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
            Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.Contrasenia),
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
            Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.Contrasenia),
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

    [Fact]
    public async Task QueSePuedaRegistrarUnCliente()
    {
        RegistrarClienteDto dto = new RegistrarClienteDto
        {
            Nombre = "Ana",
            Email = "ana@gmail.com",
            Contrasenia = "123456",
            Direccion = "Calle 1",
            Telefono = "112233"
        };
        Rol rolCliente = new Rol { Id = 1, Nombre = "cliente" };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync((Usuario)null);
        _repoMock.Setup(r => r.ObtenerRolPorNombre("cliente")).ReturnsAsync(rolCliente);

        Usuario usuarioGuardado = null;
        _repoMock.Setup(r => r.GuardarUsuarioAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => usuarioGuardado = u)
            .ReturnsAsync((Usuario u) => u);

        await _usuarioServicio.RegistrarClienteAsync(dto);

        Assert.NotNull(usuarioGuardado);
        Assert.Equal(dto.Nombre, usuarioGuardado.Nombre);
        Assert.Equal(dto.Email, usuarioGuardado.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify(dto.Contrasenia, usuarioGuardado.Contrasenia));

        Assert.NotNull(usuarioGuardado.Cliente);
        Assert.Equal(dto.Direccion, usuarioGuardado.Cliente.Direccion);
        Assert.Equal(dto.Telefono, usuarioGuardado.Cliente.NumeroTelefonico);
        Assert.Equal(0m, usuarioGuardado.Cliente.Saldo);

        Assert.Single(usuarioGuardado.UsuarioRoles);
        Assert.Equal("cliente", usuarioGuardado.UsuarioRoles.First().Rol.Nombre);

        _repoMock.Verify(r => r.GuardarUsuarioAsync(usuarioGuardado), Times.Once);
    }

    [Fact]
    public async Task SiAlRegistrarUnClienteElEmailYaExisteElServicioLanzaEmailYaRegistradoException()
    {
        RegistrarClienteDto dto = new RegistrarClienteDto
        {
            Nombre = "Ana",
            Email = "ana@gmail.com",
            Contrasenia = "123456",
            Direccion = "Calle 1",
            Telefono = "112233"
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(new Usuario { Email = dto.Email });

        await Assert.ThrowsAsync<EmailYaRegistradoException>(async () => await _usuarioServicio.RegistrarClienteAsync(dto));
    }

    [Fact]
    public async Task SiAlRegistrarUnClienteElRolNoExisteElServicioLanzaInvalidOperationException()
    {
        RegistrarClienteDto dto = new RegistrarClienteDto
        {
            Nombre = "Ana",
            Email = "ana@gmail.com",
            Contrasenia = "123456",
            Direccion = "Calle 1",
            Telefono = "112233"
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync((Usuario)null);
        _repoMock.Setup(r => r.ObtenerRolPorNombre("cliente")).ReturnsAsync((Rol)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _usuarioServicio.RegistrarClienteAsync(dto));
    }

    [Fact]
    public async Task QueSePuedaRegistrarUnRepartidor()
    {
        RegistrarRepartidorDto dto = new RegistrarRepartidorDto
        {
            Nombre = "Luis",
            Email = "luis@gmail.com",
            Contrasenia = "123456",
            Dni = "40123456",
            Direccion = "Calle 2"
        };
        Rol rolRepartidor = new Rol { Id = 2, Nombre = "repartidor" };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync((Usuario)null);
        _repoMock.Setup(r => r.ObtenerRolPorNombre("repartidor")).ReturnsAsync(rolRepartidor);

        Usuario usuarioGuardado = null;
        _repoMock.Setup(r => r.GuardarUsuarioAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => usuarioGuardado = u)
            .ReturnsAsync((Usuario u) => u);

        await _usuarioServicio.RegistrarRepartidorAsync(dto);

        Assert.NotNull(usuarioGuardado);
        Assert.Equal(dto.Nombre, usuarioGuardado.Nombre);
        Assert.Equal(dto.Email, usuarioGuardado.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify(dto.Contrasenia, usuarioGuardado.Contrasenia));

        Assert.NotNull(usuarioGuardado.Repartidor);
        Assert.Equal(dto.Dni, usuarioGuardado.Repartidor.Dni);
        Assert.Empty(usuarioGuardado.Repartidor.FotoDniUrl);
        Assert.False(usuarioGuardado.Repartidor.Verificado);

        Assert.Single(usuarioGuardado.UsuarioRoles);
        Assert.Equal("repartidor", usuarioGuardado.UsuarioRoles.First().Rol.Nombre);

        _repoMock.Verify(r => r.GuardarUsuarioAsync(usuarioGuardado), Times.Once);
    }

    [Fact]
    public async Task SiAlRegistrarUnRepartidorElEmailYaExisteElServicioLanzaEmailYaRegistradoException()
    {
        RegistrarRepartidorDto dto = new RegistrarRepartidorDto
        {
            Nombre = "Luis",
            Email = "luis@gmail.com",
            Contrasenia = "123456",
            Dni = "40123456",
            Direccion = "Calle 2"
        };

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(new Usuario { Email = dto.Email });

        await Assert.ThrowsAsync<EmailYaRegistradoException>(async () => await _usuarioServicio.RegistrarRepartidorAsync(dto));
    }

}
