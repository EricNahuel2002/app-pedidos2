namespace Usuarios.dto;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Contrasenia { get; set; } = null!;

    public LoginDto(string email, string contrasenia)
    {
        Email = email;
        Contrasenia = contrasenia;
    }
}
