using System.ComponentModel.DataAnnotations;

namespace Menus.entidad;
public class Menu
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = string.Empty;
    [Required]
    public int Precio { get; set; }
    public string? Imagen { get; set; }
}
