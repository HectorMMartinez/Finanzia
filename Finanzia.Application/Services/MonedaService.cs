using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Finanzia.Domain.DTOs;
using SkiaSharp;
using Finanzia.Application.Contract;

namespace Finanzia.Application.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly ConnectionStrings con;
        public MonedaService(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<MonedaDTO>> Lista()
        {
            List<MonedaDTO> lista = new List<MonedaDTO>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaMoneda", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new MonedaDTO()
                        {
                            IdMoneda = Convert.ToInt32(dr["IdMoneda"]),
                            Nombre = dr["Nombre"].ToString()!,
                            Simbolo = dr["Simbolo"].ToString()!,
                            FechaCreacion = DateTime.ParseExact(dr["FechaCreacion"].ToString()!, "dd/MM/yyyy", null)
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<bool> Crear(MonedaDTO moneda)
        {
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                try
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("sp_crearMoneda", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", moneda.Nombre);
                        cmd.Parameters.AddWithValue("@Simbolo", moneda.Simbolo);
                        cmd.Parameters.AddWithValue("@FechaCreacion", moneda.FechaCreacion);
                        var outputParam = new SqlParameter("@msgError", SqlDbType.VarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        // Verificar el mensaje de error del procedimiento almacenado
                        var msgError = outputParam.Value?.ToString();
                        if (!string.IsNullOrEmpty(msgError))
                        {
                            Console.WriteLine($"Error SQL al crear moneda: {msgError}");
                            return false;
                        }
                    }

                    Console.WriteLine("Moneda creada exitosamente en la base de datos.");
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error SQL: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error general: {ex.Message}");
                    return false;
                }
            }
        }

        public async Task<string> Editar(MonedaDTO objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarMoneda", conexion);
                cmd.Parameters.AddWithValue("@IdMoneda", objeto.IdMoneda);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Simbolo", objeto.Simbolo);
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

        public bool Eliminar(int id, out string mensajeError)
        {
            mensajeError = string.Empty;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_eliminarMoneda", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMoneda", id);

                    // Parámetro de salida para el mensaje de error
                    var paramError = new SqlParameter("@msgError", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramError);

                    cmd.ExecuteNonQuery();

                    // Capturar el mensaje de error devuelto
#pragma warning disable CS8601 // Possible null reference assignment.
                    mensajeError = paramError.Value.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
                    return string.IsNullOrEmpty(mensajeError); // Retorna true si no hubo error
                }
            }
        }

    }
}
