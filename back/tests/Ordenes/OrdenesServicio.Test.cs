using Moq;
using Ordenes.dto;
using Ordenes.Entidad;
using Ordenes.excepciones;
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
    private Mock<IOrdenesRepositorio> _repoMock;
    private IOrdenesServicio _ordenesServicio;

    public OrdenesServicioTest(OrdenesServicioFixture fixture)
    {
        this._repoMock = fixture.repoMock;
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

        _repoMock.Setup(r => r.ObtenerOrdenesDelClienteAsync(idUsuario)).ReturnsAsync(ordenes);

        var resultado = await _ordenesServicio.ObtenerOrdenesDelClienteAsync(idUsuario);

        Assert.Equal(ordenes[0].EmailCliente, resultado[0].EmailCliente);
    }


    [Fact]
    public async Task QueElClientePuedaConfirmarUnaOrden()
    {
        int idUsuario = 1; int idMenu = 1;
        ClienteMenuDto dto = new ClienteMenuDto(idUsuario, idMenu);

        Orden orden = new Orden { IdUsuario = idUsuario, IdMenu = idMenu, Estado = "Pendiente" };

        _repoMock.Setup(r => r.GuardarOrdenDelClienteAsync(orden));

        var resultado = await _ordenesServicio.ConfirmarOrdenDelClienteAsync(dto);

        Assert.Equal("Orden confirmada", resultado);
    }

    [Fact]
    public async Task QueElClientePuedaCancelarUnaOrden()
    {
        int idCliente = 1;
        int idOrden = 1;

        Orden orden = new Orden
        {
            Estado = "Pendiente"
        };

        _repoMock.Setup(r => r.ObtenerOrdenDelClienteAsync(idCliente, idOrden)).ReturnsAsync(orden);

        _repoMock.Setup(r => r.CancelarOrdenAsync(orden));

        var resultado = await _ordenesServicio.CancelarOrdenDelCliente(idCliente, idOrden);

        Assert.Equal("Orden cancelada", resultado);
    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenEnCursoElServicioLanzaOrdenEnCursoException()
    {
        int idCliente = 1;
        int idOrden = 1;

        Orden orden = new Orden
        {
            Estado = "En curso"
        };

        _repoMock.Setup(r => r.ObtenerOrdenDelClienteAsync(idCliente, idOrden)).ReturnsAsync(orden);

        await Assert.ThrowsAsync<OrdenEnCursoException>(async () => await _ordenesServicio.CancelarOrdenDelCliente(idCliente, idOrden));
    }


    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenYaCanceladaElServicioLanzaOrdenYaCanceladaException()
    {
        int idCliente = 1;
        int idOrden = 1;

        Orden orden = new Orden
        {
            Estado = "Cancelada"
        };

        _repoMock.Setup(r => r.ObtenerOrdenDelClienteAsync(idCliente, idOrden)).ReturnsAsync(orden);

        await Assert.ThrowsAsync<OrdenYaCanceladaException>(async () => await _ordenesServicio.CancelarOrdenDelCliente(idCliente, idOrden));
    }

}
