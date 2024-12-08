using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using Finanzia.Domain.DTOs;
using System.Globalization;
using Finanzia.Application.Contract;

namespace Finanzia.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly ConnectionStrings con;

        public PrestamoService(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Crear(PrestamoDTO objeto)
        {
            string respuesta = "";

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearPrestamo", conexion);
                cmd.Parameters.AddWithValue("@IdCliente", objeto.Cliente.IdCliente);
                cmd.Parameters.AddWithValue("@NroDocumento", objeto.Cliente.NroDocumento);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", objeto.Cliente.Apellido);
                cmd.Parameters.AddWithValue("@Correo", objeto.Cliente.Correo);
                cmd.Parameters.AddWithValue("@Telefono", objeto.Cliente.Telefono);
                cmd.Parameters.AddWithValue("@IdMoneda", objeto.Moneda.IdMoneda);
                cmd.Parameters.AddWithValue("@FechaInicio", objeto.FechaInicioPago);
                cmd.Parameters.AddWithValue("@MontoPrestamo", objeto.MontoPrestamo);
                cmd.Parameters.AddWithValue("@InteresPorcentaje", objeto.InteresPorcentaje);
                cmd.Parameters.AddWithValue("@NroCuotas", objeto.NroCuotas);
                cmd.Parameters.AddWithValue("@FormaDePago", objeto.FormaDePago);
                cmd.Parameters.AddWithValue("@ValorPorCuota", objeto.ValorPorCuota);
                cmd.Parameters.AddWithValue("@ValorInteres", objeto.ValorInteres);
                cmd.Parameters.AddWithValue("@ValorTotal", objeto.ValorTotal);
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

        public async Task<List<PrestamoDTO>> ObtenerPrestamos(int Id, string NroDocumento)
        {
            List<PrestamoDTO> lista = new List<PrestamoDTO>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerPrestamos", conexion);
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@IdPrestamo", Id);
                cmd.Parameters.AddWithValue("@NroDocumento", NroDocumento ?? string.Empty);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteXmlReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        XDocument doc = XDocument.Load(dr);

                        lista = doc.Element("Prestamos")?.Elements("Prestamo").Select(prestamo => new PrestamoDTO
                        {
                            IdPrestamo = Convert.ToInt32(prestamo.Element("IdPrestamo")?.Value ?? "0"),
                            Cliente = new ClienteDTO
                            {
                                IdCliente = Convert.ToInt32(prestamo.Element("IdCliente")?.Value ?? "0"),
                                NroDocumento = prestamo.Element("NroDocumento")?.Value ?? string.Empty,
                                Nombre = prestamo.Element("Nombre")?.Value ?? string.Empty,
                                Apellido = prestamo.Element("Apellido")?.Value ?? string.Empty,
                                Correo = prestamo.Element("Correo")?.Value ?? string.Empty,
                                Telefono = prestamo.Element("Telefono")?.Value ?? string.Empty,
                                FechaCreacion = DateTime.TryParse(prestamo.Element("FechaCreacion")?.Value, out var fechaCreacion) ? fechaCreacion : DateTime.MinValue
                            },
                            Moneda = new MonedaDTO
                            {
                                IdMoneda = Convert.ToInt32(prestamo.Element("IdMoneda")?.Value ?? "0"),
                                Nombre = prestamo.Element("NombreMoneda")?.Value ?? string.Empty,
                                Simbolo = prestamo.Element("Simbolo")?.Value ?? string.Empty
                            },
                            FechaInicioPago = DateTime.TryParse(prestamo.Element("FechaInicioPago")?.Value, out var fechaInicio) ? fechaInicio : DateTime.MinValue,
                            MontoPrestamo = decimal.TryParse(prestamo.Element("MontoPrestamo")?.Value, out var montoPrestamo) ? montoPrestamo : 0,
                            InteresPorcentaje = decimal.TryParse(prestamo.Element("InteresPorcentaje")?.Value, out var interesPorcentaje) ? interesPorcentaje : 0,
                            NroCuotas = int.TryParse(prestamo.Element("NroCuotas")?.Value, out var nroCuotas) ? nroCuotas : 0,
                            FormaDePago = prestamo.Element("FormaDePago")?.Value ?? string.Empty,
                            ValorPorCuota = decimal.TryParse(prestamo.Element("ValorPorCuota")?.Value, out var valorPorCuota) ? valorPorCuota : 0,
                            ValorInteres = decimal.TryParse(prestamo.Element("ValorInteres")?.Value, out var valorInteres) ? valorInteres : 0,
                            ValorTotal = decimal.TryParse(prestamo.Element("ValorTotal")?.Value, out var valorTotal) ? valorTotal : 0,
                            Estado = prestamo.Element("Estado")?.Value ?? string.Empty,
                            PrestamoDetalle = prestamo.Element("PrestamoDetalle")?.Elements("Detalle").Select(detalle => new PrestamoDetalleDTO
                            {
                                IdPrestamoDetalle = int.TryParse(detalle.Element("IdPrestamoDetalle")?.Value, out var idDetalle) ? idDetalle : 0,
                                FechaPago = DateTime.TryParse(detalle.Element("FechaPago")?.Value, out var fechaPago) ? fechaPago : DateTime.MinValue,
                                MontoCuota = decimal.TryParse(detalle.Element("MontoCuota")?.Value, out var montoCuota) ? montoCuota : 0,
                                NroCuota = int.TryParse(detalle.Element("NroCuota")?.Value, out var nroCuota) ? nroCuota : 0,
                                Estado = detalle.Element("Estado")?.Value ?? string.Empty,
                                FechaPagado = DateTime.TryParse(detalle.Element("FechaPagado")?.Value, out var fechaPagado) ? fechaPagado : DateTime.MinValue
                            }).ToList() ?? new List<PrestamoDetalleDTO>()
                        }).ToList() ?? new List<PrestamoDTO>();
                    }
                }
            }

            return lista;
        }

        public async Task<string> PagarCuotas(int IdPrestamo, string NroCuotasPagadas)
        {
            string respuesta = "";

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_pagarCuotas", conexion);
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@IdPrestamo", IdPrestamo);
                cmd.Parameters.AddWithValue("@NroCuotasPagadas", NroCuotasPagadas);
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
