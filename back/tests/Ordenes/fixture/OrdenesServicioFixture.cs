using Moq;
using Ordenes.repositorio;
using Ordenes.servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordenes.Test.fixture;

public class OrdenesServicioFixture
{

    public Mock<IOrdenesRepositorio> repoMock;
    public IOrdenesServicio ordenServicio;


    public OrdenesServicioFixture()
    {
        repoMock = new Mock<IOrdenesRepositorio>();
        ordenServicio = new OrdenesServicio(repoMock.Object);
    }
}
