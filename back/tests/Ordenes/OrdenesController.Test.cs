using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordenes.controller;
using Ordenes.Entidad;
using Ordenes.servicio;
using Ordenes.dto;
using Ordenes.Test.fixture;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Ordenes.Test;

public class OrdenesTest: IClassFixture<OrdenesControllerFixture>
{

    public Mock<IOrdenesServicio> ordenServicioMock;
    public OrdenesController ordenController;

    public OrdenesTest(OrdenesControllerFixture fixture)
    {
        ordenServicioMock = fixture._ordenServicioMock;
        ordenController = fixture._ordenController;
    }

    [Fact]
    public async Task SiHayOrdenesDelClienteLosRetornaYDaOkAsync()
    {
        int idCliente = 1;
        var ordenesEsperadas = new List<Orden>
        {
            new Orden{ IdOrden = 1 ,IdUsuario = idCliente, IdMenu = 1, NombreCliente = "pepe", PrecioAPagar = 50},
            new Orden{ IdOrden = 2 ,IdUsuario = idCliente, IdMenu = 3, NombreCliente = "pepe", PrecioAPagar = 30}
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, idCliente.ToString()),
            new Claim(ClaimTypes.Role,"cliente")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };


        ordenServicioMock.Setup(s => s.ObtenerOrdenesDelClienteAsync(idCliente)).ReturnsAsync(ordenesEsperadas);

        var respuesta = await ordenController.ListarOrdenesDelClienteAsync();

        var okResult = Assert.IsType<OkObjectResult>(respuesta);

        Assert.Equal(200, okResult.StatusCode);
    }


    [Fact]
    public async Task SiAlListarOrdenesDelClienteElServicioFallaRetornaHttp500() 
    {
        int idCliente = 1;
        var ordenesEsperadas = new List<Orden>
        {
            new Orden{ IdOrden = 1 ,IdUsuario = idCliente, IdMenu = 1, NombreCliente = "pepe", PrecioAPagar = 50},
            new Orden{ IdOrden = 2 ,IdUsuario = idCliente, IdMenu = 3, NombreCliente = "pepe", PrecioAPagar = 30}
        };

        ordenServicioMock.Setup(s => s.ObtenerOrdenesDelClienteAsync(idCliente)).ThrowsAsync(new Exception());

        var respuesta = await ordenController.ListarOrdenesDelClienteAsync();

        var okResult = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(500, okResult.StatusCode);

    }




    [Fact]
    public async Task QueSePuedaConfirmarUnaOrdenYRetorneCreatedAsync()
    {
        ClienteMenuDto dto = new ClienteMenuDto { IdCliente = 1, IdMenu = 1 };

        ordenServicioMock.Setup(s => s.ConfirmarOrdenDelCliente(dto)).ReturnsAsync("Orden confirmada");

        var respuesta = await ordenController.ConfirmarOrdenDelClienteAsync(dto);

        var result = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public async Task SiAlConfirmarUnaOrdenDelClienteElServicioFallaRetornaHttp500()
    {
        ClienteMenuDto dto = new ClienteMenuDto { IdCliente = 1, IdMenu = 1 };

        ordenServicioMock.Setup(s => s.ConfirmarOrdenDelCliente(dto)).ThrowsAsync(new Exception());

        var respuesta = await ordenController.ConfirmarOrdenDelClienteAsync(dto);

        var result = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(500, result.StatusCode);

    }

    [Fact]
    public async Task QueElClientePuedaCancelarUnaOrdenYRetorneOkAsync()
    {
        int idCliente = 1;
        int idOrden = 1;

        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente,idOrden)).ReturnsAsync("Orden cancelada");

        var resultado = await ordenController.CancelarOrdenDelCliente(idCliente, idOrden);

        var result = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task SiAlCancelarUnaOrdenDelClienteElServicioFallaRetornaHttp500()
    {
        int idCliente = 1;
        int idOrden = 1;

        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente, idOrden)).ThrowsAsync(new Exception());

        var resultado = await ordenController.CancelarOrdenDelCliente(idCliente, idOrden);

        var result = Assert.IsType<ObjectResult>(resultado);

        Assert.Equal(500, result.StatusCode);
    }






}
