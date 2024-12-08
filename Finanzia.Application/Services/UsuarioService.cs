using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Finanzia.Domain.DTOs;
using Finanzia.Application.Contract;
namespace Finanzia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ConnectionStrings con;
        public UsuarioService(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task RegistrarUsuario(string correo, string clave, string nombreCompleto)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(clave);
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_registrarUsuario", conexion);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Clave", hashedPassword);
                cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<UsuarioDTO> Login(string correo, string clave)
        {
            UsuarioDTO usuario = null!;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerUsuario", conexion);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Clave", clave);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        // Elimina la verificación del hash
                        usuario = new UsuarioDTO
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString()!,
                            Correo = dr["Correo"].ToString()!
                        };
                    }
                }
            }
            return usuario;
        }

    }
}
