namespace Ordenes.dto;

public class ResenasRepartidorDto
{
    public int IdRepartidor { get; set; }
    public string NombreRepartidor { get; set; } = null!;
    public double Promedio { get; set; }
    public int Cantidad { get; set; }
    public List<ResenaDto> Resenas { get; set; } = new List<ResenaDto>();
}
