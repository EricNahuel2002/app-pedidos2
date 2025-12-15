namespace Usuarios.entidad;

public class UsuarioRol
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int IdRol { get; set; }
    public Rol Rol { get; set; } = null!;
}
