namespace Usuarios.dto;

public class UsuarioAdministracionDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public bool EsCliente { get; set; }
    public bool EsRepartidor { get; set; }
    public bool RepartidorVerificado { get; set; }
}
