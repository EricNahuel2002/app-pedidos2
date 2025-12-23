using System.ComponentModel.DataAnnotations;

namespace Usuarios.dto;

public class UsuarioRepartidorDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Dni { get; set; }
    public string FotoDniUrl { get; set; } = null!;
    public bool Verificado { get; set; }
}
