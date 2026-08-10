using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using Usuarios.controlador;
using Usuarios.dto;
using Usuarios.excepciones;
using Usuarios.servicio;
using Usuarios.Test.fixture;

namespace Usuarios.Test;

public class UsuariosControladorTest: IClassFixture<UsuariosControladorFixture>
{

    private Mock<IUsuariosServicio> _servicioMock;
    private UsuariosControlador _controlador;

    public UsuariosControladorTest(UsuariosControladorFixture fixture)
    {
        _servicioMock = fixture.servicioMock;
        _controlador = fixture.controlador;
    }


    [Fact]
    public async Task SiAlValidarLasCredencialesDelUsuarioSonCorrectasRetornaHttp200()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com","123");
        UsuarioDto usuarioDto = new UsuarioDto(1, "pepe@gmail.com", "cliente");

        _servicioMock.Setup(s => s.ValidarCredencialesDeUsuario(dto)).ReturnsAsync(usuarioDto);

        var respuesta = await _controlador.ValidarCredencialesDeUsuario(dto);

        var resultado = Assert.IsType<OkObjectResult>(respuesta);
        Assert.Equal(200, resultado.StatusCode);
    }

    [Fact]
    public async Task SiAlValidarCredencialesDeUsuarioElServicioIndicaQueSonInvalidasElControladorRetornaHttp401()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");
        UsuarioDto usuarioDto = new UsuarioDto(1, "pepe@gmail.com", "cliente");

        _servicioMock.Setup(s => s.ValidarCredencialesDeUsuario(dto)).ThrowsAsync(new CredencialesInvalidasException());

        var respuesta = await _controlador.ValidarCredencialesDeUsuario(dto);

        var resultado = Assert.IsType<UnauthorizedResult>(respuesta);
        Assert.Equal(401, resultado.StatusCode);
    }

    [Fact]
    public async Task SiAlValidarLasCredencialesDelUsuarioElServicioFallaRetornaHttp500()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");

        _servicioMock.Setup(s => s.ValidarCredencialesDeUsuario(dto)).ThrowsAsync(new Exception());

        var respuesta = await _controlador.ValidarCredencialesDeUsuario(dto);

        var resultado = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(500, resultado.StatusCode);
    }


    [Fact]
    public async Task AlListarClienteElControladorRetornaOk()
    {
        int id = 1;
        UsuarioClienteDto dto = new UsuarioClienteDto { Id = id, Email = "pepe@gmail.com" };

        _servicioMock.Setup(s => s.ObtenerUsuarioCliente(id)).ReturnsAsync(dto);

        var resultado = await _controlador.ListarCliente(id);

        var httpResult = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, httpResult.StatusCode);

        Assert.NotNull(httpResult.Value);
        UsuarioClienteDto dtoQueRetorna = Assert.IsType<UsuarioClienteDto>(httpResult.Value);
        Assert.Equal(id, dtoQueRetorna.Id);
    }

    [Fact]
    public async Task AlRegistrarUnClienteElControladorRetornaHttp201()
    {
        RegistrarClienteDto dto = new RegistrarClienteDto
        {
            Nombre = "Ana",
            Email = "ana@gmail.com",
            Contrasenia = "123456",
            Direccion = "Calle 1",
            Telefono = "112233"
        };

        _servicioMock.Setup(s => s.RegistrarClienteAsync(dto)).Returns(Task.CompletedTask);

        var resultado = await _controlador.RegistrarCliente(dto);

        var objectResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task SiAlRegistrarUnClienteElEmailYaExisteElControladorRetornaHttp409()
    {
        RegistrarClienteDto dto = new RegistrarClienteDto
        {
            Nombre = "Ana",
            Email = "ana@gmail.com",
            Contrasenia = "123456",
            Direccion = "Calle 1",
            Telefono = "112233"
        };

        _servicioMock.Setup(s => s.RegistrarClienteAsync(dto)).ThrowsAsync(new EmailYaRegistradoException());

        var resultado = await _controlador.RegistrarCliente(dto);

        var conflict = Assert.IsType<ConflictObjectResult>(resultado);
        Assert.Equal(409, conflict.StatusCode);
    }

    [Fact]
    public async Task AlRegistrarUnRepartidorElControladorRetornaHttp201()
    {
        RegistrarRepartidorDto dto = new RegistrarRepartidorDto
        {
            Nombre = "Luis",
            Email = "luis@gmail.com",
            Contrasenia = "123456",
            Dni = "40123456",
            Direccion = "Calle 2"
        };

        _servicioMock.Setup(s => s.RegistrarRepartidorAsync(dto)).Returns(Task.CompletedTask);

        var resultado = await _controlador.RegistrarRepartidor(dto);

        var objectResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task SiAlRegistrarUnRepartidorElEmailYaExisteElControladorRetornaHttp409()
    {
        RegistrarRepartidorDto dto = new RegistrarRepartidorDto
        {
            Nombre = "Luis",
            Email = "luis@gmail.com",
            Contrasenia = "123456",
            Dni = "40123456",
            Direccion = "Calle 2"
        };

        _servicioMock.Setup(s => s.RegistrarRepartidorAsync(dto)).ThrowsAsync(new EmailYaRegistradoException());

        var resultado = await _controlador.RegistrarRepartidor(dto);

        var conflict = Assert.IsType<ConflictObjectResult>(resultado);
        Assert.Equal(409, conflict.StatusCode);
    }
}
