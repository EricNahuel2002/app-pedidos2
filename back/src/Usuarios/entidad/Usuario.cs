using System.ComponentModel.DataAnnotations;

namespace Usuarios.entidad;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Contrasenia { get; set; } = null!;

    public Cliente? Cliente { get; set; }
    public Repartidor? Repartidor { get; set; }
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}
