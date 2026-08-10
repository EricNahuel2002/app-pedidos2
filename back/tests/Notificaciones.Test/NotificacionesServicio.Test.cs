using Moq;
using Notificaciones.dto;
using Notificaciones.servicio;
using System.Net;
using System.Net.Http.Json;

namespace Notificaciones.Test;

public class NotificacionesServicioTest
{
    private static NotificacionesServicio CrearServicio(HttpResponseMessage respuesta)
    {
        var handler = new FakeHttpMessageHandler(respuesta);
        var cliente = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var fabrica = new Mock<IHttpClientFactory>();
        fabrica.Setup(f => f.CreateClient("Apigateway")).Returns(cliente);

        return new NotificacionesServicio(fabrica.Object);
    }

    [Fact]
    public async Task AlMarcarUnaOrdenComoFinalizadaRetornaElMensajeDeExito()
    {
        var servicio = CrearServicio(new HttpResponseMessage(HttpStatusCode.OK));

        var mensaje = await servicio.MarcarOrdenComoFinalizada(1, 5);

        Assert.Equal("Orden con id:5 finalizada", mensaje);
    }

    [Fact]
    public async Task SiElServicioDeOrdenesFallaAlMarcarUnaOrdenComoFinalizadaSeLanzaHttpRequestException()
    {
        var servicio = CrearServicio(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
            await servicio.MarcarOrdenComoFinalizada(1, 5));
    }

    [Fact]
    public async Task AlObtenerOrdenesPendientesDevuelveLasOrdenesDelServicioDeOrdenes()
    {
        var orden = new OrdenDto
        {
            IdOrden = 1,
            IdUsuario = 1,
            IdMenu = 1,
            NombreMenu = "Empanadas",
            NombreCliente = "Pepe",
            EmailCliente = "pepe@gmail.com",
            PrecioAPagar = 10,
            Estado = "Pendiente",
            Direccion = "Calle 1"
        };
        var respuesta = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new List<OrdenDto> { orden })
        };
        var servicio = CrearServicio(respuesta);

        var ordenes = await servicio.ObtenerOrdenesPendientes();

        Assert.Single(ordenes);
        Assert.Equal(1, ordenes[0].IdOrden);
        Assert.Equal("Empanadas", ordenes[0].NombreMenu);
    }

    [Fact]
    public async Task SiElServicioDeOrdenesFallaAlObtenerOrdenesPendientesSeLanzaHttpRequestException()
    {
        var servicio = CrearServicio(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
            await servicio.ObtenerOrdenesPendientes());
    }

    [Fact]
    public async Task AlTomarUnaOrdenConExitoRetornaElMensajeDeExito()
    {
        var servicio = CrearServicio(new HttpResponseMessage(HttpStatusCode.OK));

        var mensaje = await servicio.TomarUnaOrden(1, 5);

        Assert.Equal("Orden tomada exitosamente", mensaje);
    }

    [Fact]
    public async Task AlTomarUnaOrdenQueYaFueTomadaRetornaElMensajeDeError()
    {
        var servicio = CrearServicio(new HttpResponseMessage(HttpStatusCode.Conflict));

        var mensaje = await servicio.TomarUnaOrden(1, 5);

        Assert.Equal("Orden ya tomada por otro repartidor o ya finalizada", mensaje);
    }
}
