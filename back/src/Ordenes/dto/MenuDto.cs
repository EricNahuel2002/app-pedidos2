namespace Ordenes.dto;

public class MenuDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = string.Empty;
    public int Precio { get; set; }
    public string Imagen { get; set; } = null!;
}
