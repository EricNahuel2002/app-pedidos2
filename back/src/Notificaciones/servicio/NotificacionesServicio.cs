using Notificaciones.dto;
using Notificaciones.entidad;
using Notificaciones.excepciones;
using Notificaciones.repositorio;

namespace Notificaciones.servicio;

public interface INotificacionesServicio
{
    Task<List<NotificacionDto>> ObtenerNotificacionesDelUsuarioAsync(int idUsuario);
    Task CrearNotificacionAsync(int idUsuario, string mensaje);
    Task MarcarNotificacionComoLeidaAsync(int idUsuario, int idNotificacion);
}

public class NotificacionesServicio : INotificacionesServicio
{
    private INotificacionesRepositorio _repo;

    public NotificacionesServicio(INotificacionesRepositorio repo)
    {
        _repo = repo;
    }

    public async Task<List<NotificacionDto>> ObtenerNotificacionesDelUsuarioAsync(int idUsuario)
    {
        var notificaciones = await _repo.ObtenerNotificacionesDelUsuarioAsync(idUsuario);

        return notificaciones.Select(n => new NotificacionDto
        {
            IdNotificacion = n.IdNotificacion,
            IdUsuario = n.IdUsuario,
            Mensaje = n.Mensaje,
            Leida = n.Leida,
            FechaCreacion = n.FechaCreacion
        }).ToList();
    }

    public async Task CrearNotificacionAsync(int idUsuario, string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
        {
            throw new InvalidOperationException("El mensaje de la notificación no puede estar vacío");
        }

        var notificacion = new Notificacion
        {
            IdUsuario = idUsuario,
            Mensaje = mensaje,
            Leida = false,
            FechaCreacion = DateTime.UtcNow
        };

        await _repo.GuardarNotificacionAsync(notificacion);
    }

    public async Task MarcarNotificacionComoLeidaAsync(int idUsuario, int idNotificacion)
    {
        var notificacion = await _repo.ObtenerNotificacionAsync(idUsuario, idNotificacion);

        if (notificacion == null)
        {
            throw new NotificacionNoEncontradaException($"Notificación con idNotificacion: {idNotificacion} no encontrada para el usuario: {idUsuario}");
        }

        notificacion.Leida = true;

        await _repo.ActualizarNotificacionAsync(notificacion);
    }
}
