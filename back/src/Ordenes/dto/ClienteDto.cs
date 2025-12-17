using System.ComponentModel.DataAnnotations;

namespace Ordenes.dto;

public class ClienteDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public decimal Saldo { get; set; }
    public string NumeroTelefonico { get; set; } = null!;
}
