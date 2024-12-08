namespace Finanzia.Domain.Entities
{
    public class Moneda
    {
        public int IdMoneda { get; set; }
        public string Nombre { get; set; } = null!;
        public string Simbolo { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
