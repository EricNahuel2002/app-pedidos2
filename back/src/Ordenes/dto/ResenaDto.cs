namespace Ordenes.dto;

public class ResenaDto
{
    public int Id { get; set; }
    public int IdOrden { get; set; }
    public int IdCliente { get; set; }
    public int IdRepartidor { get; set; }
    public string NombreCliente { get; set; } = null!;
    public string NombreRepartidor { get; set; } = null!;
    public int Puntaje { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaCreacion { get; set; }
}
