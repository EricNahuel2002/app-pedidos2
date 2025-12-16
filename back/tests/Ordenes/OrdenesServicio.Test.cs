using Moq;
using Ordenes.Entidad;
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
    public async Task QueSePuedanObtenerOrdenesDelClienteAsync()
    {
        int idUsuario = 1;
        List<Orden> ordenes = new List<Orden>
        {
            new Orden{ IdOrden = 1, IdUsuario = idUsuario, IdMenu = 1, NombreCliente = "Eric"
            , EmailCliente = "ericaquino2002@gmail.com", Direccion = "Lamadrid",
                PrecioAPagar = 50, Estado = "Pendiente", FechaOrden = DateTime.UtcNow}
        };

        _repo.Setup(r => r.ObtenerOrdenesDelClienteAsync(idUsuario)).ReturnsAsync(ordenes);

        var resultado = await _ordenesServicio.ObtenerOrdenesDelClienteAsync(idUsuario);

        Assert.Equal(ordenes[0].EmailCliente, resultado[0].EmailCliente);
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
