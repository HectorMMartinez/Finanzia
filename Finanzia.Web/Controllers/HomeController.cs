using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finanzia.Web.Models;
using System.Diagnostics;
using Finanzia.Domain.DTOs;
using Finanzia.Application.Contract;

namespace Prestamo.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IResumenService _resumenService;

        public HomeController(ILogger<HomeController> logger, IResumenService resumenService)
        {
            _logger = logger;
            _resumenService = resumenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            ResumenDTO objeto = await _resumenService.ObtenerResumen();
            return StatusCode(StatusCodes.Status200OK, new { data = objeto });
        }
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
