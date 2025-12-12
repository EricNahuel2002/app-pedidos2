using Menus.entidad;
using Menus.repositorio;
using Menus.servicio;
using Menus.Test.Fixture;
using Moq;

namespace Menus.Test;


public class MenusServicioTests:IClassFixture<MenuServicioFixture>
{
    private readonly Mock<IMenuRepositorio> _menuRepoMock;
    public IMenuServicio _menuServicio;

    public MenusServicioTests(MenuServicioFixture fixture)
    {
        _menuRepoMock = new Mock<IMenuRepositorio>();
        _menuServicio = new MenuServicio(_menuRepoMock.Object);
    }

    [Fact]
    public async Task SiHayMenusLosRetorna()
    {
        var menus = new List<Menu>
        {
            new Menu { Id = 1, Nombre = "Menu1" },
            new Menu { Id = 2, Nombre = "Menu2" }
        };

        _menuRepoMock.Setup(repo => repo.ObtenerMenusAsync())
            .ReturnsAsync(menus);

        var resultado = await _menuServicio.ObtenerMenusAsync();

        Assert.IsType<List<Menu>>(resultado);

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task SiNoHayMenusRetornaListaVacia()
    {
        var menus = new List<Menu>();
        _menuRepoMock.Setup(repo => repo.ObtenerMenusAsync())
            .ReturnsAsync(menus);
        var resultado = await _menuServicio.ObtenerMenusAsync();
        Assert.IsType<List<Menu>>(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task SiMenuExisteLoRetorna()
    {
        int idMenu = 1;
        Menu menu = new Menu { Id = idMenu, Nombre = "Menu1" };

        _menuRepoMock.Setup(repo => repo.ObtenerMenuAsync(idMenu))
            .ReturnsAsync(menu);

        var resultado = await _menuServicio.ObtenerMenuAsync(idMenu);
        Assert.IsType<Menu>(resultado);
        Assert.Equal(idMenu, resultado.Id);
    }

    [Fact]
    public async Task SiMenuNoExisteRetornaNull()
    {
        int idMenu = 1;
        Menu menu = new Menu { Id = 12, Nombre = "Menu1" };
        _menuRepoMock.Setup(repo => repo.ObtenerMenuAsync(idMenu))
            .ReturnsAsync((Menu?)null);
        var resultado = await _menuServicio.ObtenerMenuAsync(idMenu);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task SiAlObtenerMenusElRepositorioLanzaErrorServicioLoPropaga()
    {
        _menuRepoMock.Setup(repo => repo.ObtenerMenusAsync()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await _menuServicio.ObtenerMenusAsync());
    }

    [Fact]
    public async Task SiAlObtenerMenuElRepositorioLanzaErrorServicioLoPropaga()
    {
        int idMenu = 1;
        _menuRepoMock.Setup(repo => repo.ObtenerMenuAsync(idMenu)).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await _menuServicio.ObtenerMenuAsync(idMenu));
    }


    [Fact]
    public async Task QueSePuedaCrearUnMenu()
    {
        Menu menu = new Menu { Nombre = "Hamburguesa", Descripcion = "Rica hamburguesa" };

        _menuRepoMock.Setup(repo => repo.CrearMenuAsync(menu)).ReturnsAsync(menu.Id);

        var resultado = await _menuServicio.CrearMenuAsync(menu);
        Assert.Equal(menu.Id, resultado);
    }

}
