using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class PagoDetalleDTO
    {
        public int IdPrestamoDetalle { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        public decimal MontoCuota { get; set; }

        [Required]
        public string? Cliente { get; set; }

        [Required]
        public string? Cuota { get; set; }
    }

}
