namespace Notificaciones.excepciones;

public class NotificacionNoEncontradaException : Exception
{
    public NotificacionNoEncontradaException() { }

    public NotificacionNoEncontradaException(string mensaje) : base(mensaje) { }
}
