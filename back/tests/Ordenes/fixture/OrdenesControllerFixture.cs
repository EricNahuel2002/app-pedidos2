using Moq;
using Ordenes.controller;
using Ordenes.servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordenes.Test.fixture;

public class OrdenesControllerFixture
{
    public Mock<IOrdenesServicio> _ordenServicioMock;
    public OrdenesController _ordenController;

    public OrdenesControllerFixture()
    {
        _ordenServicioMock = new Mock<IOrdenesServicio>();
        _ordenController = new OrdenesController(_ordenServicioMock.Object);
    }
}
