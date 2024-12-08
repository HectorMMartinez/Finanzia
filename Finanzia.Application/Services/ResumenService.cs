using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Finanzia.Domain.DTOs;
using Finanzia.Application.Contract;
using Finanzia.Application;

namespace Prestamo.Application.Services
{
    public class ResumenService : IResumenService
    {
        private readonly ConnectionStrings con;
        public ResumenService(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<ResumenDTO> ObtenerResumen()
        {
            ResumenDTO resumen = new ResumenDTO
            {
                IngresosPorMes = new List<IngresoMensualDTO>(),
                PagosProximos = new List<PagoDetalleDTO>(),
                PagosAtrasados = new List<PagoDetalleDTO>()
            };

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerResumen", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    // Leer Resumen General
                    if (await dr.ReadAsync())
                    {
                        resumen.TotalClientes = dr["TotalClientes"]?.ToString() ?? "0";
                        resumen.PrestamosPendientes = dr["PrestamosPendientes"]?.ToString() ?? "0";
                        resumen.PrestamosCancelados = dr["PrestamosCancelados"]?.ToString() ?? "0";
                        resumen.InteresAcumulado = dr["InteresAcumulado"]?.ToString() ?? "0";
                        resumen.MontoTotalPrestado = dr["MontoTotalPrestado"]?.ToString() ?? "0";
                        resumen.TotalPagosRecibidos = dr["TotalPagosRecibidos"]?.ToString() ?? "0";
                        resumen.TasaInteresesPromedio = dr["TasaInteresesPromedio"]?.ToString() ?? "0";
                        resumen.ClientesActivos = dr["ClientesActivos"]?.ToString() ?? "0";
                    }

                    // Leer Ingresos por Mes
                    if (await dr.NextResultAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            resumen.IngresosPorMes.Add(new IngresoMensualDTO
                            {
                                Mes = dr["Mes"]?.ToString() ?? "Sin Mes",
                                Ingreso = dr["Ingreso"] != DBNull.Value ? Convert.ToDecimal(dr["Ingreso"]) : 0
                            });
                        }
                    }

                    // Leer Pagos Próximos
                    if (await dr.NextResultAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            resumen.PagosProximos.Add(new PagoDetalleDTO
                            {
                                FechaPago = dr.GetDateTime(1),
                                MontoCuota = dr.GetDecimal(2),
                                Cliente = dr.GetString(3),
                                Cuota = dr.GetString(4)
                            });
                        }
                    }

                    // Leer Pagos Atrasados
                    if (await dr.NextResultAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            resumen.PagosAtrasados.Add(new PagoDetalleDTO
                            {
                                FechaPago = dr.GetDateTime(1),
                                MontoCuota = dr.GetDecimal(2),
                                Cliente = dr.GetString(3),
                                Cuota = dr.GetString(4)
                            });
                        }
                    }
                }
            }
            return resumen;
        }
    }
}
