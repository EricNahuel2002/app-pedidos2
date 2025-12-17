using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordenes.Entidad
{
    public class Orden
    {
        [Key]
        public int IdOrden { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public int IdMenu { get; set; }
        [Required]
        public string NombreMenu { get; set; } = null!;
        [Required]
        [MaxLength(150)] 
        public string NombreCliente { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string EmailCliente { get; set; } = null!;

        [Required]
        public int PrecioAPagar { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "pendiente";

        [MaxLength(500)]
        [Required]
        public string Direccion { get; set; } = null!;
        public DateTime FechaOrden { get; set; } = DateTime.UtcNow;
    }
}