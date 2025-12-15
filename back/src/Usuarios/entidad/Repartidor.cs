namespace Usuarios.entidad;

public class Repartidor
{
    public int IdUsuario { get; set; }
    public int Dni { get; set; }
    public string FotoDniUrl { get; set; } = null!;
    public bool Verificado { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
