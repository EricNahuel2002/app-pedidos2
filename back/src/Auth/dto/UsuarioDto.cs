
namespace Auth.dto;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;
}
