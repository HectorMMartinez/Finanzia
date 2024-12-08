namespace Finanzia.Domain.Entities
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public Moneda Moneda { get; set; } = new Moneda();
        public DateTime? FechaInicioPago { get; set; } = null!;
        public decimal MontoPrestamo { get; set; } = 0;
        public decimal? InteresPorcentaje { get; set; } = null!;
        public int NroCuotas { get; set; }
        public string FormaDePago { get; set; } = null!;
        public decimal? ValorPorCuota { get; set; } = null!;
        public decimal? ValorInteres { get; set; } = null!;
        public decimal? ValorTotal { get; set; } = null!;
        public string Estado { get; set; } = "Pendiente";
        public DateTime? FechaCreacion { get; set; } = null!;
        public List<PrestamoDetalle> PrestamoDetalle { get; set; } = null!;
    }
}
