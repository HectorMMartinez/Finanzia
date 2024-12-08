using System.ComponentModel.DataAnnotations;

namespace Finanzia.Domain.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Correo { get; set; }

        public string? Rol { get; set; }
        public bool Activo { get; set; }
    }
}
