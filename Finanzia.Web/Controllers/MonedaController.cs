using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Finanzia.Domain.DTOs;

namespace Finanzia.Web.Controllers
{
    public class MonedaController : Controller
    {
        private readonly HttpClient _httpClient;

        public MonedaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PrestamoApi");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var monedas = await _httpClient.GetFromJsonAsync<List<MonedaDTO>>("Moneda");
            return Json(new { data = monedas });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] MonedaDTO moneda)
        {
            var response = await _httpClient.PostAsJsonAsync("Moneda", moneda);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] MonedaDTO moneda)
        {
            var response = await _httpClient.PutAsJsonAsync("Moneda", moneda);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"Moneda/{id}");
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }
    }
}

