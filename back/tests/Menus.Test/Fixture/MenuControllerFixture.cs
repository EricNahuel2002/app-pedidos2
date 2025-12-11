using Menus.controlador;
using Menus.entidad;
using Menus.servicio;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus.Test.Fixture;

public class MenuControllerFixture
{
    public Mock<IMenuServicio> _menuServicioMock;
    public MenuController _menuController;

    public MenuControllerFixture()
    {
        _menuServicioMock = new Mock<IMenuServicio>();

        _menuController = new MenuController(_menuServicioMock.Object);
    }
}
