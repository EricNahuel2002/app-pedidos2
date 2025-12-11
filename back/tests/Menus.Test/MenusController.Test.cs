using Menus.controlador;
using Menus.entidad;
using Menus.servicio;
using Menus.Test.Fixture;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Menus.Test;

public class MenusControllerTests: IClassFixture<MenuControllerFixture>
{
    public Mock<IMenuServicio> _menuServicioMock;
    public MenuController _menuController;

    public MenusControllerTests(MenuControllerFixture fixture)
    {
        _menuServicioMock = fixture._menuServicioMock;
        _menuController = fixture._menuController;
    }

    [Fact]
    public async Task SiHayMenusDisponiblesLosRetornaYDaOkAsync()
    {
        var menusEsperados = new List<Menu>
        {
            new Menu { Id = 1, Nombre = "Pizza", Descripcion = "Rica pizza" },
            new Menu { Id = 2, Nombre = "Papas fritas", Descripcion = "Ricas papas fritas" }
        };

        _menuServicioMock.Setup(s => s.ObtenerMenusAsync()).ReturnsAsync(menusEsperados);

        var resultado = await _menuController.GetMenusAsync();

        var okResult = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, okResult.StatusCode);

        var menusObtenidos = Assert.IsType<List<Menu>>(okResult.Value);

        Assert.Equal(2, menusObtenidos.Count);

        Assert.Equal(menusEsperados[0].Id, menusObtenidos[0].Id);

    }


    [Fact]
    public async Task SiNoHayMenusDisponiblesRetornaListaVaciaYOkAsync()
    {
        var menusEsperados = new List<Menu>{};

        _menuServicioMock.Setup(s => s.ObtenerMenusAsync()).ReturnsAsync(menusEsperados);

        var resultado = await _menuController.GetMenusAsync();

        var okResult = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, okResult.StatusCode);

        var menusObtenidos = Assert.IsType<List<Menu>>(okResult.Value);
        
        Assert.Empty(menusObtenidos);

    }


    [Fact]
    public async Task SiExisteElMenuIngresadoRetornaElObjetoYOk()
    {
        Menu menu = new Menu { Id = 1, Nombre = "Pizza", Descripcion = "Rica pizza" };

        _menuServicioMock.Setup(s => s.ObtenerMenuAsync(menu.Id)).ReturnsAsync(menu);

        var resultado = await _menuController.GetMenuAsync(menu.Id);

        var okResult = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, okResult.StatusCode);

        var menuObtenido = Assert.IsType<Menu>(okResult.Value);

        Assert.Equal(menu.Id, menuObtenido.Id);
    }

    [Fact]
    public async Task SiNoExisteElMenuIngresadoRetornaVacioYOkAsync()
    {
        int idMenu = 999;
        _menuServicioMock.Setup(s => s.ObtenerMenuAsync(idMenu)).ReturnsAsync((Menu?)null);
        var resultado = await _menuController.GetMenuAsync(idMenu);
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Null(okResult.Value);
    }

    [Fact]
    public async Task SiGetMenusAsyncRecibeUnaExcepcionDelServicioRetornaHttp500()
    {
        _menuServicioMock.Setup(s => s.ObtenerMenusAsync()).ThrowsAsync(new Exception());

        var resultado =  await _menuController.GetMenusAsync();

        var statusCodeResult = Assert.IsType<ObjectResult>(resultado);

        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task SiGetMenuAsyncRecibeUnaExcepcionDelServicioRetornaHttp500()
    {
        int idMenu = 1;
        _menuServicioMock.Setup(s => s.ObtenerMenuAsync(idMenu)).ThrowsAsync(new Exception());
        var resultado = await _menuController.GetMenuAsync(idMenu);
        var statusCodeResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task QueSePuedaCrearUnMenu()
    {
        Menu menu =new Menu { Id = 1, Nombre = "Hamburguesa", Descripcion = "Rica hamburguesa" };

        _menuServicioMock.Setup(s => s.CrearMenuAsync(menu)).ReturnsAsync(menu.Id);

        var resultado = await _menuController.CrearMenuAsync(menu);

        var okStatus = Assert.IsType<OkObjectResult>(resultado);

        Assert.Equal(200, okStatus.StatusCode);
        Assert.Equal(menu.Id, okStatus.Value);

    }
}
