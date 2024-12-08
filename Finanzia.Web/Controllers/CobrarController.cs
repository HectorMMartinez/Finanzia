using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Finanzia.Web.Controllers
{
    public class CobrarController : Controller
    {
        private readonly HttpClient _httpClient;

        public CobrarController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PrestamoApi");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("PagarCuotas")]
        public async Task<IActionResult> PagarCuotas(int idPrestamo, string nroCuotasPagadas)
        {
            var response = await _httpClient.PostAsync($"Prestamo/PagarCuotas?idPrestamo={idPrestamo}&nroCuotasPagadas={nroCuotasPagadas}", null);
            var resultado = await response.Content.ReadAsStringAsync();
            return Json(new { data = resultado });
        }
    }
}

