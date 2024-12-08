namespace Finanzia.Domain.Entities
{
    public class Resumen
    {
        public string? TotalClientes { get; set; }
        public string? PrestamosPendientes { get; set; }
        public string? PrestamosCancelados { get; set; }
        public string? InteresAcumulado { get; set; }
        public string? MontoTotalPrestado { get; set; }
        public string? TotalPagosRecibidos { get; set; }
        public string? TasaInteresesPromedio { get; set; }
        public string? ClientesActivos { get; set; }
        public List<IngresoMensual> IngresosPorMes { get; set; }
    }


}
