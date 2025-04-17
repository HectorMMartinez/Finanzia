using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class ClienteDTO
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "El número de documento debe tener el formato dominicano (###-#######-#).")]
        public string? NroDocumento { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "El teléfono debe seguir el formato 000-000-0000.")]
        public string? Telefono { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
