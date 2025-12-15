using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using Usuarios.controlador;
using Usuarios.dto;
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

        var usuario = Assert.IsType<UsuarioDto>(resultado.Value);
        Assert.Equal(1, usuario.Id);
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
}
