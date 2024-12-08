namespace Finanzia.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
