using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class PrestamoDTO
    {
        public int IdPrestamo { get; set; }

        [Required]
        public ClienteDTO Cliente { get; set; } = null!;

        [Required]
        public MonedaDTO Moneda { get; set; } = null!;

        [Required]
        public DateTime FechaInicioPago { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto del préstamo debe ser mayor que 0.")]
        public decimal MontoPrestamo { get; set; }

        [Range(0.01, 100, ErrorMessage = "El interés debe estar entre 0.01% y 100%.")]
        public decimal InteresPorcentaje { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El número de cuotas debe ser mayor que 0.")]
        public int NroCuotas { get; set; }

        [Required]
        public string FormaDePago { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "El valor por cuota debe ser mayor que 0.")]
        public decimal ValorPorCuota { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El valor del interés debe ser mayor que 0.")]
        public decimal ValorInteres { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El valor total debe ser mayor que 0.")]
        public decimal ValorTotal { get; set; }

        [Required]
        public string Estado { get; set; } = null!;

        public List<PrestamoDetalleDTO> PrestamoDetalle { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
    }
}
