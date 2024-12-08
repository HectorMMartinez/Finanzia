using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Finanzia.Domain.DTOs;
using System.Globalization;
using Finanzia.Application.Contract;

namespace Finanzia.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ConnectionStrings con;
        public ClienteService(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<ClienteDTO>> Lista()
        {
            List<ClienteDTO> lista = new List<ClienteDTO>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaCliente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new ClienteDTO()
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            NroDocumento = dr["NroDocumento"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,
                            Apellido = dr["Apellido"].ToString()!,
                            Correo = dr["Correo"].ToString()!,
                            Telefono = dr["Telefono"].ToString()!,
                            FechaCreacion = string.IsNullOrWhiteSpace(dr["FechaCreacion"].ToString())
                            ? DateTime.MinValue // O algún valor predeterminado
                            : DateTime.ParseExact(dr["FechaCreacion"].ToString()!, "dd/MM/yyyy", null)

                        });
                    }
                }
            }
            return lista;
        }
        public async Task<List<ClienteDTO>> ObtenerPorUsuario(int idUsuario)
        {
            var clientes = new List<ClienteDTO>();
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerClientesPorUsuario", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        clientes.Add(new ClienteDTO
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                        });
                    }
                }
            }
            return clientes;
        }
        public async Task<ClienteDTO?> ObtenerClientePorDocumento(string nroDocumento)
        {
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                using (var command = new SqlCommand("SELECT * FROM Cliente WHERE NroDocumento = @NroDocumento", conexion))
                {
                    command.Parameters.AddWithValue("@NroDocumento", nroDocumento);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ClienteDTO
                            {
                                IdCliente = reader.GetInt32(0),
                                NroDocumento = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Correo = reader.GetString(4),
                                Telefono = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null; // Si no encuentra el cliente
        }


        public async Task<ClienteDTO> Obtener(string NroDocumento)
        {
            ClienteDTO objeto = new ClienteDTO();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerCliente", conexion);
                cmd.Parameters.AddWithValue("@NroDocumento", NroDocumento);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new ClienteDTO()
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            NroDocumento = dr["NroDocumento"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,
                            Apellido = dr["Apellido"].ToString()!,
                            Correo = dr["Correo"].ToString()!,
                            Telefono = dr["Telefono"].ToString()!,
                            FechaCreacion = string.IsNullOrWhiteSpace(dr["FechaCreacion"].ToString())
                            ? DateTime.MinValue // O algún valor predeterminado
                            : DateTime.ParseExact(dr["FechaCreacion"].ToString()!, "dd/MM/yyyy", null)
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<string> Crear(ClienteDTO objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearCliente", conexion);
                cmd.Parameters.AddWithValue("@NroDocumento", objeto.NroDocumento);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", objeto.Apellido);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Telefono", objeto.Telefono);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }

        public async Task<string> Editar(ClienteDTO objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarCliente", conexion);
                cmd.Parameters.AddWithValue("@IdCliente", objeto.IdCliente);
                cmd.Parameters.AddWithValue("@NroDocumento", objeto.NroDocumento);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", objeto.Apellido);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Telefono", objeto.Telefono);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }

        public async Task<string> Eliminar(int Id)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarCliente", conexion);
                cmd.Parameters.AddWithValue("@IdCliente", Id);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }
    }
}
