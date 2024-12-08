using Finanzia.Domain.DTOs;

namespace Finanzia.Application.Contract
{
    public interface IResumenService
    {
        /// <summary>
        /// Obtiene un resumen general del sistema, incluyendo métricas, ingresos por mes, pagos próximos y pagos atrasados.
        /// </summary>
        /// <returns>Objeto DTO con los datos del resumen.</returns>
        Task<ResumenDTO> ObtenerResumen();
    }
}

