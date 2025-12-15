
namespace Usuarios.dto;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;

    public UsuarioDto(int id, string email, string rol)
    {
        Id = id;
        Email = email;
        Rol = rol;
    }
}
