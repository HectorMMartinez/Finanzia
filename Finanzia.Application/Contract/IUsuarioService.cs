using Finanzia.Domain.DTOs;

namespace Finanzia.Application.Contract
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario.</param>
        /// <param name="clave">Clave del usuario (se almacenará encriptada).</param>
        /// <param name="nombreCompleto">Nombre completo del usuario.</param>
        Task RegistrarUsuario(string correo, string clave, string nombreCompleto);

        /// <summary>
        /// Valida las credenciales de un usuario y devuelve su información si es válido.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario.</param>
        /// <param name="clave">Clave del usuario.</param>
        /// <returns>Objeto UsuarioDTO con la información del usuario.</returns>
        Task<UsuarioDTO> Login(string correo, string clave);
    }
}

