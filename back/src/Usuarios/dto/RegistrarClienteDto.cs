namespace Usuarios.dto;

public class RegistrarClienteDto
{
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Contrasenia { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
}
