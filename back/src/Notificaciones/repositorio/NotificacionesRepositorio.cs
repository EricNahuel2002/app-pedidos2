using Microsoft.EntityFrameworkCore;
using Notificaciones.contexto;
using Notificaciones.entidad;

namespace Notificaciones.repositorio;

public interface INotificacionesRepositorio
{
    Task<List<Notificacion>> ObtenerNotificacionesDelUsuarioAsync(int idUsuario);
    Task<Notificacion?> ObtenerNotificacionAsync(int idUsuario, int idNotificacion);
    Task<Notificacion> GuardarNotificacionAsync(Notificacion notificacion);
    Task<Notificacion> ActualizarNotificacionAsync(Notificacion notificacion);
}

public class NotificacionesRepositorio : INotificacionesRepositorio
{
    private NotificacionesDbContext _ctx;

    public NotificacionesRepositorio(NotificacionesDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Notificacion>> ObtenerNotificacionesDelUsuarioAsync(int idUsuario)
    {
        return await _ctx.Notificaciones
            .Where(n => n.IdUsuario == idUsuario)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Notificacion?> ObtenerNotificacionAsync(int idUsuario, int idNotificacion)
    {
        return await _ctx.Notificaciones
            .Where(n => n.IdUsuario == idUsuario && n.IdNotificacion == idNotificacion)
            .FirstOrDefaultAsync();
    }

    public async Task<Notificacion> GuardarNotificacionAsync(Notificacion notificacion)
    {
        _ctx.Notificaciones.Add(notificacion);
        await _ctx.SaveChangesAsync();
        return notificacion;
    }

    public async Task<Notificacion> ActualizarNotificacionAsync(Notificacion notificacion)
    {
        _ctx.Notificaciones.Update(notificacion);
        await _ctx.SaveChangesAsync();
        return notificacion;
    }
}
