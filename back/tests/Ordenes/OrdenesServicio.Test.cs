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
    private List<string> _notificacionesEnviadas;

    public OrdenesServicioTest(OrdenesServicioFixture fixture)
    {
        this._repoMock = fixture.repoMock;
        this._ordenesServicio = fixture.ordenServicio;
        this._notificacionesEnviadas = fixture.notificacionesEnviadas;
        this._notificacionesEnviadas.Clear();
    }


    [Fact]
    public async Task QueSePuedanObtenerOrdenesDelClienteAsync()
    {
        int idUsuario = 1;
        List<Orden> ordenes = new List<Orden>
        {
            new Orden{ IdOrden = 1, IdCliente = idUsuario, IdMenu = 1, NombreCliente = "Eric"
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

        Orden orden = new Orden { IdCliente = idUsuario, IdMenu = idMenu, Estado = "Pendiente" };

        _repoMock.Setup(r => r.GuardarOrdenDelClienteAsync(orden));

        var resultado = await _ordenesServicio.ConfirmarOrdenDelClienteAsync(idUsuario,idMenu);

        Assert.Equal("Orden confirmada", resultado);
    }

    [Fact]
    public async Task QueElClientePuedaCancelarUnaOrden()
    {
        int idCliente = 1;
        int idOrden = 1;

        Orden orden = new Orden
        {
            Estado = "PENDIENTE"
        };

        _repoMock.Setup(r => r.ObtenerOrdenDelClienteAsync(idCliente, idOrden)).ReturnsAsync(orden);

        _repoMock.Setup(r => r.ActualizarEstadoDeOrden(orden));

        var resultado = await _ordenesServicio.CancelarOrdenDelCliente(idCliente, idOrden);

        Assert.Equal("CANCELADA", orden.Estado);

        Assert.Equal("Orden cancelada", resultado);
    }

    [Fact]
    public async Task SiSeIntentaCancelarUnaOrdenEnCursoElServicioLanzaOrdenEnCursoException()
    {
        int idCliente = 1;
        int idOrden = 1;

        Orden orden = new Orden
        {
            Estado = "EN CURSO"
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
            Estado = "CANCELADA"
        };

        _repoMock.Setup(r => r.ObtenerOrdenDelClienteAsync(idCliente, idOrden)).ReturnsAsync(orden);

        await Assert.ThrowsAsync<OrdenYaCanceladaException>(async () => await _ordenesServicio.CancelarOrdenDelCliente(idCliente, idOrden));
    }

    [Fact]
    public async Task QueAlConfirmarUnaOrdenSeNotifiqueAlCliente()
    {
        int idUsuario = 1; int idMenu = 1;

        _repoMock.Setup(r => r.GuardarOrdenDelClienteAsync(It.IsAny<Orden>()));

        await _ordenesServicio.ConfirmarOrdenDelClienteAsync(idUsuario, idMenu);

        var notificacion = Assert.Single(_notificacionesEnviadas);
        Assert.Contains("confirmado", notificacion);
        Assert.Contains("\"idUsuario\":1", notificacion);
    }

    [Fact]
    public async Task QueAlTomarUnaOrdenSeNotifiqueAlCliente()
    {
        int idUsuario = 1; int idOrden = 1;

        Orden orden = new Orden
        {
            IdOrden = idOrden,
            IdCliente = 3,
            NombreMenu = "Menu 1",
            Estado = "PENDIENTE"
        };

        _repoMock.Setup(r => r.ObtenerOrden(idOrden)).ReturnsAsync(orden);
        _repoMock.Setup(r => r.ActualizarEstadoDeOrden(It.IsAny<Orden>()));

        var resultado = await _ordenesServicio.TomarUnaOrden(idUsuario, idOrden);

        Assert.Equal("Orden tomada exitosamente", resultado);
        var notificacion = Assert.Single(_notificacionesEnviadas);
        Assert.Contains("tomado", notificacion);
        Assert.Contains("\"idUsuario\":3", notificacion);
    }

    [Fact]
    public async Task QueAlMarcarUnaOrdenComoFinalizadaSeNotifiqueAlCliente()
    {
        int idUsuario = 1; int idOrden = 1;

        Orden orden = new Orden
        {
            IdOrden = idOrden,
            IdCliente = 3,
            NombreMenu = "Menu 1",
            Estado = "EN CURSO",
            IdRepartidor = idUsuario
        };

        _repoMock.Setup(r => r.ObtenerOrdenTomadaPorRepartidorAsync(idUsuario, idOrden)).ReturnsAsync(orden);
        _repoMock.Setup(r => r.ActualizarEstadoDeOrden(It.IsAny<Orden>()));

        var resultado = await _ordenesServicio.MarcarOrdenComoFinalizada(idUsuario, idOrden);

        Assert.Equal("Orden finalizada", resultado);
        var notificacion = Assert.Single(_notificacionesEnviadas);
        Assert.Contains("finalizado", notificacion);
        Assert.Contains("\"idUsuario\":3", notificacion);
    }

}
