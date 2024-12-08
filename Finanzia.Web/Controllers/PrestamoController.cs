using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finanzia.Domain.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finanzia.Web.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {

        private readonly HttpClient _httpClient;

        public PrestamoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PrestamoApi");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Nuevo()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerCliente(string NroDocumento)
        {
            var cliente = await _httpClient.GetFromJsonAsync<ClienteDTO>($"Cliente/{NroDocumento}");
            return Json(new { data = cliente });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PrestamoDTO prestamoDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Prestamo", prestamoDto);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPrestamos(int IdPrestamo, string NroDocumento)
        {
            var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoDTO>>($"Prestamo/{IdPrestamo}/{NroDocumento}");
            return Json(new { data = prestamos });
        }


        [HttpGet]
        public async Task<IActionResult> ImprimirPrestamo(int IdPrestamo)
        {
            var response = await _httpClient.GetAsync($"Prestamo/{IdPrestamo}/GenerarPDF");
            if (response.IsSuccessStatusCode)
            {
                var pdfStream = await response.Content.ReadAsStreamAsync();
                return File(pdfStream, "application/pdf");
            }
            return StatusCode((int)response.StatusCode, "Error al generar el PDF");
        }

    }
}
