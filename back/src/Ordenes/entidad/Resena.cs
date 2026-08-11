using System.ComponentModel.DataAnnotations;

namespace Ordenes.Entidad
{
    public class Resena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdOrden { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdRepartidor { get; set; }

        [Required]
        [MaxLength(150)]
        public string NombreCliente { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string NombreRepartidor { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Puntaje { get; set; }

        [MaxLength(500)]
        public string? Comentario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
