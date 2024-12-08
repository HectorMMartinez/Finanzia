using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Finanzia.Domain.DTOs;

namespace Finanzia.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClienteController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();

            var apiBaseUrl = configuration["ApiBaseUrl"];
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ArgumentNullException(nameof(apiBaseUrl), "ApiBaseUrl no está configurado en appsettings.json.");
            }
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var clientes = await _httpClient.GetFromJsonAsync<List<ClienteDTO>>("Cliente");
            return Json(new { data = clientes });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ClienteDTO cliente)
        {
            var response = await _httpClient.PostAsJsonAsync("Cliente", cliente);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] ClienteDTO cliente)
        {
            var response = await _httpClient.PutAsJsonAsync("Cliente", cliente);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"Cliente/{id}");
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }
    }
}
