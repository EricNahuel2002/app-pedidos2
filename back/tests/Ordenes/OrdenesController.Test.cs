using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordenes.controller;
using Ordenes.Entidad;
using Ordenes.servicio;
using Ordenes.dto;
using Ordenes.Test.fixture;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

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
            new Claim(JwtRegisteredClaimNames.Sub, idCliente.ToString()),
            new Claim("role","cliente")
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
    public async Task SiAlListarLasOrdenesDelClienteLaClaimDelIdUsuarioNoExisteElControladorRetornaHttp401()
    {
        int idCliente = 1;
        var ordenesEsperadas = new List<Orden>
        {
            new Orden{ IdOrden = 1 ,IdUsuario = idCliente, IdMenu = 1, NombreCliente = "pepe", PrecioAPagar = 50},
            new Orden{ IdOrden = 2 ,IdUsuario = idCliente, IdMenu = 3, NombreCliente = "pepe", PrecioAPagar = 30}
        };

        var claims = new List<Claim>
        {
            new Claim("role","cliente")
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

        ordenServicioMock.Setup(s => s.ObtenerOrdenesDelClienteAsync(idCliente));

        var resultado = await ordenController.ListarOrdenesDelClienteAsync();

        var httpResult = Assert.IsType<UnauthorizedResult>(resultado);

        Assert.Equal(401, httpResult.StatusCode);

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

        ordenServicioMock.Setup(s => s.ObtenerOrdenesDelClienteAsync(idCliente)).ThrowsAsync(new Exception());

        var respuesta = await ordenController.ListarOrdenesDelClienteAsync();

        var okResult = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(500, okResult.StatusCode);

    }




    [Fact]
    public async Task QueSePuedaConfirmarUnaOrdenYRetorneCreatedAsync()
    {
        int idCliente = 1;
        int idMenu = 1;
        ClienteMenuDto dto = new ClienteMenuDto(idCliente,idMenu);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, idCliente.ToString()),
            new Claim("role","cliente")
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

        ordenServicioMock.Setup(s => s.ConfirmarOrdenDelClienteAsync(dto)).ReturnsAsync("Orden confirmada");

        var respuesta = await ordenController.ConfirmarOrdenDelClienteAsync(idMenu);

        var result = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(201, result.StatusCode);
    }


    [Fact]
    public async Task SiAlConfirmarUnaOrdenElClaimDelIdUsuarioNoExisteElControladorRetornaHttp401()
    {
        int idCliente = 1;
        string rol = "cliente";
        int idMenu = 1;

        ClienteMenuDto dto = new ClienteMenuDto(idCliente, idMenu);

        var claims = new List<Claim>
        {
            new Claim("role",rol)
        };

        var identity = new ClaimsIdentity(claims, "testAuth");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };

        ordenServicioMock.Setup(s => s.ConfirmarOrdenDelClienteAsync(dto));

        var resultado = await ordenController.ConfirmarOrdenDelClienteAsync(idMenu);

        var httpResult = Assert.IsType<UnauthorizedResult>(resultado);

        Assert.Equal(401, httpResult.StatusCode);
    }






    [Fact]
    public async Task SiAlConfirmarUnaOrdenDelClienteElServicioFallaRetornaHttp500()
    {
        int idCliente = 1;
        int idMenu = 1;
        ClienteMenuDto dto = new ClienteMenuDto(idCliente, idMenu); ;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, idCliente.ToString()),
            new Claim("role","cliente")
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

        ordenServicioMock.Setup(s => s.ConfirmarOrdenDelClienteAsync(dto)).ThrowsAsync(new Exception());

        var respuesta = await ordenController.ConfirmarOrdenDelClienteAsync(idMenu);

        var result = Assert.IsType<ObjectResult>(respuesta);

        Assert.Equal(500, result.StatusCode);

    }

    [Fact]
    public async Task QueElClientePuedaCancelarUnaOrdenYRetorneOkAsync()
    {
        int idCliente = 1;
        int idOrden = 1;
        string rol = "cliente";

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,idCliente.ToString()),
            new Claim("role",rol)
        };

        var identity = new ClaimsIdentity(claims, "TestOrdenes");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };

        CancelarOrdenRequestDto dto = new CancelarOrdenRequestDto { IdOrden = idOrden };


        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente,idOrden)).ReturnsAsync("Orden cancelada");

        var resultado = await ordenController.CancelarOrdenDelCliente(dto);

        var result = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task SiAlCancelarUnaOrdenDelClienteElServicioFallaRetornaHttp500()
    {
        int idCliente = 1;
        int idOrden = 1;
        string rol = "cliente";

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,idCliente.ToString()),
            new Claim("role",rol)
        };

        var identity = new ClaimsIdentity(claims, "TestOrdenes");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };

        CancelarOrdenRequestDto dto = new CancelarOrdenRequestDto { IdOrden = idOrden };

        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente, idOrden)).ThrowsAsync(new Exception());

        var resultado = await ordenController.CancelarOrdenDelCliente(dto);

        var result = Assert.IsType<ObjectResult>(resultado);

        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task SiAlCancelarUnaOrdenDelClienteElDtoQueLlegaEsNuloElControladorRetornaHttp404()
    {
        int idCliente = 1;
        string rol = "cliente";
        int idOrden = 1;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,idCliente.ToString()),
            new Claim("role",rol)
        };

        var identity = new ClaimsIdentity(claims, "testAuth");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };


        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente, idOrden));

        var resultado = await ordenController.CancelarOrdenDelCliente(null);

        var httpStatusCode = Assert.IsType<NotFoundResult>(resultado);

        Assert.Equal(404, httpStatusCode.StatusCode);
    }

    [Fact]
    public async Task SiAlCancelarUnaOrdenDelClienteLaClaimDelIdDelUsuarioQueLlegaNoExisteElControladorRetornaHttp401()
    {
        int idCliente = 1;
        string rol = "cliente";
        int idOrden = 1;

        var claims = new List<Claim>
        {
            new Claim("role",rol)
        };

        var identity = new ClaimsIdentity(claims, "testAuth");
        var user = new ClaimsPrincipal(identity);

        ordenController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };


        ordenServicioMock.Setup(s => s.CancelarOrdenDelCliente(idCliente, idOrden));

        CancelarOrdenRequestDto dto = new CancelarOrdenRequestDto { IdOrden = idOrden };

        var resultado = await ordenController.CancelarOrdenDelCliente(dto);

        var httpStatusCode = Assert.IsType<UnauthorizedResult>(resultado);

        Assert.Equal(401, httpStatusCode.StatusCode);
    }







}
