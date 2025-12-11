using Menus.Context;
using Menus.entidad;
using Menus.Test.Fixture;
using Microsoft.EntityFrameworkCore;

namespace Menus.Test;

public class MenusRepositorioTests: IClassFixture<MenuRepositorioFixture>
{
    private readonly DbContextOptions<MenuDbContext> _options;

    public MenusRepositorioTests(MenuRepositorioFixture fixture)
    {
        _options = fixture._ctx;
    }


    [Fact]
    public async Task QueSePuedanObtenerMenus()
    {
        using(var ctx = new MenuDbContext(_options))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            ctx.Menus.AddRange(
                new Menu { Id = 1, Nombre = "Pizza" },
                new Menu { Id = 2, Nombre = "Pancho" },
                new Menu { Id = 3, Nombre = "Ravioles" });
            await ctx.SaveChangesAsync();
        }
        using(var ctx = new MenuDbContext(_options))
        {
            var menus = await ctx.Menus.ToListAsync();

            Assert.NotEmpty(menus);
            Assert.Equal(3, menus.Count);
        }
    }

    [Fact]
    public async Task QueSePuedaObtenerUnMenu()
    {
        using(var ctx = new MenuDbContext(_options))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();
            ctx.Menus.Add(new Menu { Id = 1, Nombre = "Pizza" });
            await ctx.SaveChangesAsync();
        }
        using(var ctx = new MenuDbContext(_options))
        {
            int idMenu = 1;
            var menu = await ctx.Menus.Where(m => m.Id == idMenu).FirstOrDefaultAsync();
            Assert.NotNull(menu);
            Assert.Equal("Pizza", menu.Nombre);
        }
    }

    [Fact]
    public async Task QueSePuedaCrearUnMenu()
    {
        using(var ctx = new MenuDbContext(_options))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();
        }
        using(var ctx = new MenuDbContext(_options))
        {
            var nuevoMenu = new Menu { Id = 1, Nombre = "Pizza" };
            ctx.Menus.Add(nuevoMenu);
            await ctx.SaveChangesAsync();
        }
        using(var ctx = new MenuDbContext(_options))
        {
            var menuCreado = await ctx.Menus.Where(m => m.Id == 1).FirstOrDefaultAsync();
            Assert.NotNull(menuCreado);
            Assert.Equal("Pizza", menuCreado.Nombre);
        }
    }
}
