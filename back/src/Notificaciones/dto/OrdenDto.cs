using System.ComponentModel.DataAnnotations;

namespace Notificaciones.dto;

public class OrdenDto
{
    public int IdOrden { get; set; }
    public int IdUsuario { get; set; }
    public int IdMenu { get; set; }
    public string NombreMenu { get; set; } = null!;
    public string NombreCliente { get; set; } = null!;
    public string EmailCliente { get; set; } = null!;
    public int PrecioAPagar { get; set; }
    public string Estado { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public DateTime FechaOrden { get; set; } = DateTime.UtcNow;
}
