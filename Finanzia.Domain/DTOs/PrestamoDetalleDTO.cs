using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class PrestamoDetalleDTO
    {
        public int IdPrestamoDetalle { get; set; }
        public int IdPrestamo { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El número de cuota debe ser mayor que 0.")]
        public int NroCuota { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la cuota debe ser mayor que 0.")]
        public decimal MontoCuota { get; set; }

        public string? Estado { get; set; }
        public DateTime? FechaPagado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
