using Finanzia.Domain.DTOs;

namespace Finanzia.Application.Contract
{
    public interface IClienteService
    {
        Task<List<ClienteDTO>> Lista();
        Task<List<ClienteDTO>> ObtenerPorUsuario(int idUsuario);
        Task<ClienteDTO?> ObtenerClientePorDocumento(string nroDocumento);
        Task<ClienteDTO> Obtener(string nroDocumento);
        Task<string> Crear(ClienteDTO cliente);
        Task<string> Editar(ClienteDTO cliente);
        Task<string> Eliminar(int idCliente);
    }

}