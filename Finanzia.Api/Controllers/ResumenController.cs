using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Finanzia.Application.Contract;
namespace Finanzia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumenController : ControllerBase
    {
        private readonly IResumenService _resumenService;

        public ResumenController(IResumenService resumenService)
        {
            _resumenService = resumenService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resumen = await _resumenService.ObtenerResumen();
            return Ok(new { data = resumen });
        }

    }
}
