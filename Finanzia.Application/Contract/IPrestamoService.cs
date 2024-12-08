using Finanzia.Domain.DTOs;

namespace Finanzia.Application.Contract
{
    public interface IPrestamoService
    {
        /// <summary>
        /// Crea un nuevo préstamo en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto DTO del préstamo a crear.</param>
        /// <returns>Cadena indicando el resultado del proceso.</returns>
        Task<string> Crear(PrestamoDTO objeto);

        /// <summary>
        /// Obtiene una lista de préstamos basados en el ID o número de documento.
        /// </summary>
        /// <param name="Id">ID del préstamo.</param>
        /// <param name="NroDocumento">Número de documento del cliente.</param>
        /// <returns>Lista de objetos DTO de préstamos.</returns>
        Task<List<PrestamoDTO>> ObtenerPrestamos(int Id, string NroDocumento);

        /// <summary>
        /// Procesa el pago de cuotas de un préstamo específico.
        /// </summary>
        /// <param name="IdPrestamo">ID del préstamo.</param>
        /// <param name="NroCuotasPagadas">Números de las cuotas a pagar, separados por comas.</param>
        /// <returns>Cadena indicando el resultado del proceso.</returns>
        Task<string> PagarCuotas(int IdPrestamo, string NroCuotasPagadas);
    }
}

