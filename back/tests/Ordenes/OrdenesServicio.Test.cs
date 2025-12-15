using Moq;
using Ordenes.repositorio;
using Ordenes.servicio;
using Ordenes.Test.fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordenes.Test;

public class OrdenesServicioTest : IClassFixture<OrdenesServicioFixture>
{
    private Mock<IOrdenesRepositorio> _repo;
    private IOrdenesServicio _ordenesServicio;

    public OrdenesServicioTest(OrdenesServicioFixture fixture)
    {
        this._repo = fixture.repoMock;
        this._ordenesServicio = fixture.ordenServicio;
    }


    [Fact]
    public async Task QueSePuedanObtenerOrdenesDelCliente()
    {

    }

    [Fact]
    public async Task SiAlObtenerOrdenesDelClienteElClienteNoExisteELServicioLanzaClienteInexistenteException()
    {

    }

    [Fact]
    public async Task SiAlObtenerOrdenesDelClienteNoHayOrdenesRetornaNull()
    {

    }

    [Fact]
    public async Task SiAlObtenerOrdenesDelClienteElRepositorioDaErrorElServicioLoPropaga()
    {

    }




    [Fact]
    public async Task QueElClientePuedaConfirmarUnaOrden()
    {

    }

    [Fact]
    public async Task SiElClienteIntentaConfirmarUnaOrdenYSuIdNoExisteElServicioLanzaClienteInexistenteException()
    {

    }

    [Fact]
    public async Task SiElClienteIntentaConfirmarUnaOrdenYElIdOrdenNoExisteElServicioLanzaOrdenInexistenteException()
    {

    }

    [Fact]
    public async Task SiAlConfirmarUnaOrdenElRepositorioDaErrorElServicioLoPropaga()
    {

    }

    [Fact]
    public async Task QueElClientePuedaCancelarUnaOrden()
    {

    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenEnCursoElServicioLanzaOrdenEnCursoException()
    {

    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenYaCanceladaElServicioLanzaOrdenYaCanceladaException()
    {

    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenYElIdClienteNoExisteElServicioLanzaClienteInexistenteException()
    {

    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenYElIdOrdenNoExisteElServicioLanzaOrdenInexistenteException()
    {

    }
}
