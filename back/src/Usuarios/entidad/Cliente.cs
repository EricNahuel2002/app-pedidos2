namespace Usuarios.entidad;

public class Cliente
{
    public int IdUsuario { get; set; }
    public string Direccion { get; set; } = null!;
    public decimal Saldo { get; set; }
    public string NumeroTelefonico { get; set; } = null!;

    public Usuario Usuario { get; set; } = null!;
}
