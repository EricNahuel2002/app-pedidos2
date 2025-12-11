using Menus.Context;
using Menus.repositorio;
using Microsoft.EntityFrameworkCore;


namespace Menus.Test.Fixture;

public class MenuRepositorioFixture
{
    public DbContextOptions<MenuDbContext> _ctx;
    public IMenuRepositorio menuRepo;

    public MenuRepositorioFixture()
    {
        _ctx = new DbContextOptionsBuilder<MenuDbContext>()
            .UseInMemoryDatabase(databaseName: "menu_db" + Guid.NewGuid().ToString())
            .Options;
        menuRepo = new MenuRepositorio(new MenuDbContext(_ctx));
    }
}
