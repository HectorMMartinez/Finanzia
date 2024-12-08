using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Asegúrate de tener esta referencia
using Finanzia.Application.Contract;
using Finanzia.Domain.DTOs;

namespace Finanzia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonedaController : ControllerBase
    {
        private readonly IMonedaService _monedaService;

        public MonedaController(IMonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var monedas = await _monedaService.Lista();
            return Ok(new { data = monedas });
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] MonedaDTO monedaDto)
        {
            if (monedaDto == null)
            {
                return BadRequest(new { message = "Los datos enviados son inválidos." });
            }
            if (monedaDto.FechaCreacion == DateTime.MinValue)
            {
                monedaDto.FechaCreacion = DateTime.Now;
            }
            var resultado = await _monedaService.Crear(monedaDto);
            if (resultado)
            {
                return Ok(new { data = "Moneda registrada correctamente" });
            }
            else
            {
                return BadRequest(new { message = resultado });
            }
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] MonedaDTO monedaDto)
        {
            var respuesta = await _monedaService.Editar(monedaDto);
            return Ok(new { data = respuesta });
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            string mensajeError = string.Empty;

            // Llamar al procedimiento almacenado
            var resultado = _monedaService.Eliminar(id, out mensajeError);

            if (!string.IsNullOrEmpty(mensajeError))
            {
                return BadRequest(new { data = mensajeError }); // Enviar el mensaje de error
            }

            if (resultado)
            {
                return Ok(new { data = "" }); // Operación exitosa
            }

            return StatusCode(500, new { data = "Error inesperado al eliminar la moneda." }); // Error inesperado


        }
    }
}
