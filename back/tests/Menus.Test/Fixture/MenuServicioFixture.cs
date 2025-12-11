using Menus.repositorio;
using Menus.servicio;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus.Test.Fixture;

public class MenuServicioFixture
{
    private readonly Mock<IMenuRepositorio> _menuRepoMock;
    public IMenuServicio _menuServicio;

    public MenuServicioFixture()
    {
        _menuRepoMock = new Mock<IMenuRepositorio>();
        _menuServicio = new MenuServicio(_menuRepoMock.Object);
    }
}
