using Moq;
using Notificaciones.entidad;
using Notificaciones.excepciones;
using Notificaciones.repositorio;
using Notificaciones.servicio;

namespace Notificaciones.Test;

public class NotificacionesServicioTest
{
    private readonly Mock<INotificacionesRepositorio> _repoMock;
    private readonly NotificacionesServicio _servicio;

    public NotificacionesServicioTest()
    {
        _repoMock = new Mock<INotificacionesRepositorio>();
        _servicio = new NotificacionesServicio(_repoMock.Object);
    }

    [Fact]
    public async Task AlObtenerNotificacionesDelUsuarioDevuelveLasNotificacionesDelRepositorio()
    {
        var notificaciones = new List<Notificacion>
        {
            new Notificacion { IdNotificacion = 1, IdUsuario = 1, Mensaje = "Tu pedido fue tomado", Leida = false, FechaCreacion = DateTime.UtcNow }
        };

        _repoMock.Setup(r => r.ObtenerNotificacionesDelUsuarioAsync(1)).ReturnsAsync(notificaciones);

        var resultado = await _servicio.ObtenerNotificacionesDelUsuarioAsync(1);

        Assert.Single(resultado);
        Assert.Equal(1, resultado[0].IdNotificacion);
        Assert.Equal("Tu pedido fue tomado", resultado[0].Mensaje);
    }

    [Fact]
    public async Task AlCrearUnaNotificacionGuardaUnaNotificacionNueva()
    {
        await _servicio.CrearNotificacionAsync(5, "Tu pedido fue confirmado");

        _repoMock.Verify(r => r.GuardarNotificacionAsync(It.Is<Notificacion>(n =>
            n.IdUsuario == 5 &&
            n.Mensaje == "Tu pedido fue confirmado" &&
            n.Leida == false)), Times.Once);
    }

    [Fact]
    public async Task SiElMensajeEstaVacioAlCrearUnaNotificacionSeLanzaInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _servicio.CrearNotificacionAsync(5, "   "));
    }

    [Fact]
    public async Task AlMarcarUnaNotificacionComoLeidaActualizaLaNotificacion()
    {
        var notificacion = new Notificacion { IdNotificacion = 3, IdUsuario = 1, Mensaje = "Tu pedido fue finalizado", Leida = false };

        _repoMock.Setup(r => r.ObtenerNotificacionAsync(1, 3)).ReturnsAsync(notificacion);

        await _servicio.MarcarNotificacionComoLeidaAsync(1, 3);

        Assert.True(notificacion.Leida);
        _repoMock.Verify(r => r.ActualizarNotificacionAsync(notificacion), Times.Once);
    }

    [Fact]
    public async Task SiLaNotificacionNoExisteAlMarcarComoLeidaSeLanzaNotificacionNoEncontradaException()
    {
        _repoMock.Setup(r => r.ObtenerNotificacionAsync(1, 99)).ReturnsAsync((Notificacion?)null);

        await Assert.ThrowsAsync<NotificacionNoEncontradaException>(async () =>
            await _servicio.MarcarNotificacionComoLeidaAsync(1, 99));
    }
}
