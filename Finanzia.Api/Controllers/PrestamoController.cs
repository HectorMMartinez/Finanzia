using Microsoft.AspNetCore.Mvc;
using Finanzia.Application.Contract;
using Finanzia.Domain.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
namespace Finanzia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;
        private readonly IClienteService _clienteService;

        public PrestamoController(IPrestamoService prestamoService, IClienteService clienteService)
        {
            _prestamoService = prestamoService;
            _clienteService = clienteService;
        }
        [HttpGet("ObtenerCliente")]
        public async Task<IActionResult> ObtenerCliente(string NroDocumento)
        {
            if (string.IsNullOrEmpty(NroDocumento))
            {
                return BadRequest(new { message = "El número de documento es requerido." });
            }

            var cliente = await _clienteService.ObtenerClientePorDocumento(NroDocumento);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado." });
            }

            return Ok(new { data = cliente });
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] PrestamoDTO prestamoDto)
        {
            try
            {
                var respuesta = await _prestamoService.Crear(prestamoDto);
                if (!string.IsNullOrEmpty(respuesta))
                {
                    return BadRequest(new { message = respuesta });
                }

                return Ok(new { message = "Préstamo registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al registrar el préstamo.", details = ex.Message });
            }
        }
        [HttpGet("{IdPrestamo}/GenerarPDF")]
        public async Task<IActionResult> GenerarPDF(int IdPrestamo)
        {
            var prestamo = await _prestamoService.ObtenerPrestamos(IdPrestamo, string.Empty);
            List<PrestamoDTO> lista = await _prestamoService.ObtenerPrestamos(IdPrestamo, "");
            PrestamoDTO objeto = lista[0];

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {

                    page.Margin(30);

                    page.Header().ShowOnce().Background("#D5D5D5").Padding(3).Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text("Préstamo").Bold().FontSize(14);
                        row.RelativeItem().AlignRight().Text($"Nro: {objeto.IdPrestamo}").Bold().FontSize(14);
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Spacing(18);

                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Datos del Cliente").Bold().FontSize(12);
                            });
                            col2.Item().Row(row =>
                            {

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Numero Documento: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Cliente.NroDocumento ?? "N/A").FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Nombre: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Cliente.Nombre ?? "N/A").FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Apellido: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Cliente.Apellido ?? "N/A").FontSize(12);
                                    });
                                });
                                row.ConstantItem(50);
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Correo: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Cliente.Correo ?? "N/A").FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Telefono: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Cliente.Telefono ?? "N/A").FontSize(12);
                                    });
                                });
                            });
                        });

                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Datos del Préstamo").Bold().FontSize(12);
                            });

                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Monto Prestado: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.MontoPrestamo.ToString()).FontSize(12);

                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Interes %: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.InteresPorcentaje.ToString()).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Número Cuotas: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.NroCuotas.ToString()).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Forma de pago: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.FormaDePago).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Tipo Moneda: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.Moneda.Nombre).FontSize(12);
                                    });
                                });
                                row.ConstantItem(50);
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Monto por Cuota: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.ValorPorCuota.ToString()).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Monto Interes: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.ValorInteres.ToString()).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Monto Total: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.ValorTotal.ToString()).FontSize(12);
                                    });
                                });
                            });
                        });


                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Detalle Cuotas").Bold().FontSize(12);
                            });
                            col2.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#D5D5D5")
                                    .Padding(4).Text("Nro. Cuota").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Fecha de Pago").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Monto a Pagar").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Estado").FontColor("#000");
                                });

                                foreach (var item in objeto.PrestamoDetalle)
                                {


                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                        .Padding(4).Text(item.NroCuota.ToString()).FontSize(12);

                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).Text(item.FechaPago).FontSize(12);

                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).Text($"{objeto.Moneda.Simbolo} {item.MontoCuota}").FontSize(12);

                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).AlignRight().Text($"{item.Estado}").FontSize(12);
                                }

                            });
                        });

                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();


            Stream pdfStream = new MemoryStream(pdf);
            return File(pdfStream, "application/pdf");
        }
        [HttpGet("ObtenerPrestamos")]
        public async Task<IActionResult> ObtenerPrestamos(
      [FromQuery] int idPrestamo = 0,
      [FromQuery] string nroDocumento = "")
        {
            try
            {
                var prestamos = await _prestamoService.ObtenerPrestamos(idPrestamo, nroDocumento);

                if (prestamos == null || !prestamos.Any())
                {
                    return NotFound(new { message = "No se encontraron préstamos para el documento proporcionado." });
                }

                return Ok(prestamos);
            }
            catch (Exception ex)
            {
                // Logea el detalle completo del error
                Console.WriteLine($"Error en ObtenerPrestamos: {ex.Message} \n {ex.StackTrace}");
                return StatusCode(500, new { message = "Error al obtener los préstamos.", details = ex.Message });
            }
        }


        [HttpPost("PagarCuotas")]
        public async Task<IActionResult> PagarCuotas(int idPrestamo, string nroCuotasPagadas)
        {
            var respuesta = await _prestamoService.PagarCuotas(idPrestamo, nroCuotasPagadas);

            if (string.IsNullOrEmpty(respuesta))
            {
                return Ok(new { success = true, message = "Pago registrado correctamente." });
            }
            else
            {
                return BadRequest(new { success = false, message = respuesta });
            }
        }

    }
}

