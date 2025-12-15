using System.ComponentModel.DataAnnotations;

namespace Usuarios.entidad;

public class Rol
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; } = null!;
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}
