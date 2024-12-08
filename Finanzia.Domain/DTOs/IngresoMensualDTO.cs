using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class IngresoMensualDTO
    {
        [Required]
        public string Mes { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "El ingreso debe ser un valor positivo.")]
        public decimal Ingreso { get; set; }
    }
}
