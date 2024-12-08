using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class MonedaDTO
    {
        public int IdMoneda { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El símbolo es obligatorio.")]
        [StringLength(10, ErrorMessage = "El símbolo no puede tener más de 10 caracteres.")]
        public string Simbolo { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }
    }
}
