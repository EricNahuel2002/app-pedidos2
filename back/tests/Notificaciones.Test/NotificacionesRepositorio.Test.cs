using Microsoft.EntityFrameworkCore;
using Notificaciones.contexto;
using Notificaciones.entidad;
using Notificaciones.repositorio;

namespace Notificaciones.Test;

public class NotificacionesRepositorioTest
{
    private static DbContextOptions<NotificacionesDbContext> CrearOpciones()
        => new DbContextOptionsBuilder<NotificacionesDbContext>()
            .UseInMemoryDatabase(databaseName: "notificaciones_db_" + Guid.NewGuid().ToString("N"))
            .Options;

    private static INotificacionesRepositorio CrearRepositorio(DbContextOptions<NotificacionesDbContext> opciones)
        => new NotificacionesRepositorio(new NotificacionesDbContext(opciones));

    [Fact]
    public async Task QueSePuedanGuardarYObtenerNotificacionesDelUsuario()
    {
        var opciones = CrearOpciones();

        using (var ctx = new NotificacionesDbContext(opciones))
        {
            ctx.Notificaciones.AddRange(
                new Notificacion { IdUsuario = 1, Mensaje = "Notificación 1", Leida = false },
                new Notificacion { IdUsuario = 1, Mensaje = "Notificación 2", Leida = false },
                new Notificacion { IdUsuario = 2, Mensaje = "De otro usuario", Leida = false }
            );
            await ctx.SaveChangesAsync();
        }

        var repo = CrearRepositorio(opciones);
        var notificaciones = await repo.ObtenerNotificacionesDelUsuarioAsync(1);

        Assert.Equal(2, notificaciones.Count);
        Assert.All(notificaciones, n => Assert.Equal(1, n.IdUsuario));
    }

    [Fact]
    public async Task QueSePuedaObtenerUnaNotificacionDeUnUsuario()
    {
        var opciones = CrearOpciones();

        using (var ctx = new NotificacionesDbContext(opciones))
        {
            ctx.Notificaciones.Add(new Notificacion { IdUsuario = 1, Mensaje = "Notificación 1", Leida = false });
            await ctx.SaveChangesAsync();
        }

        var repo = CrearRepositorio(opciones);
        var notificacion = await repo.ObtenerNotificacionAsync(1, 1);

        Assert.NotNull(notificacion);
        Assert.Equal("Notificación 1", notificacion.Mensaje);
    }

    [Fact]
    public async Task SiLaNotificacionEsDeOtroUsuarioDevuelveNull()
    {
        var opciones = CrearOpciones();

        using (var ctx = new NotificacionesDbContext(opciones))
        {
            ctx.Notificaciones.Add(new Notificacion { IdUsuario = 1, Mensaje = "Notificación 1", Leida = false });
            await ctx.SaveChangesAsync();
        }

        var repo = CrearRepositorio(opciones);
        var notificacion = await repo.ObtenerNotificacionAsync(2, 1);

        Assert.Null(notificacion);
    }

    [Fact]
    public async Task QueSePuedaMarcarUnaNotificacionComoLeida()
    {
        var opciones = CrearOpciones();

        using (var ctx = new NotificacionesDbContext(opciones))
        {
            ctx.Notificaciones.Add(new Notificacion { IdUsuario = 1, Mensaje = "Notificación 1", Leida = false });
            await ctx.SaveChangesAsync();
        }

        var repo = CrearRepositorio(opciones);
        var notificacion = await repo.ObtenerNotificacionAsync(1, 1);
        notificacion!.Leida = true;
        await repo.ActualizarNotificacionAsync(notificacion);

        using (var ctx = new NotificacionesDbContext(opciones))
        {
            var actualizada = await ctx.Notificaciones.FindAsync(1);
            Assert.True(actualizada!.Leida);
        }
    }
}
