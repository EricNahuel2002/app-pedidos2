namespace Notificaciones.dto;

public class NotificacionDto
{
    public int IdNotificacion { get; set; }
    public int IdUsuario { get; set; }
    public string Mensaje { get; set; } = null!;
    public bool Leida { get; set; }
    public DateTime FechaCreacion { get; set; }
}
