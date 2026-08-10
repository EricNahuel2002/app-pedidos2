using System.ComponentModel.DataAnnotations;

namespace Notificaciones.entidad;

public class Notificacion
{
    [Key]
    public int IdNotificacion { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    [MaxLength(500)]
    public string Mensaje { get; set; } = null!;

    public bool Leida { get; set; } = false;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
