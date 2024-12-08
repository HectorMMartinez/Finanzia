using Finanzia.Domain.DTOs;

namespace Finanzia.Application.Contract
{
    public interface IMonedaService
    {
        Task<List<MonedaDTO>> Lista();
        Task<bool> Crear(MonedaDTO moneda);
        Task<string> Editar(MonedaDTO objeto);
        bool Eliminar(int id, out string mensajeError);
    }
}

