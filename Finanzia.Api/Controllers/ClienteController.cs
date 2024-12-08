using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Finanzia.Application.Contract;
using Finanzia.Domain.DTOs;

namespace Finanzia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var clientes = await _clienteService.Lista();
                return Ok(new { data = clientes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los clientes.", details = ex.Message });
            }
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null || string.IsNullOrEmpty(clienteDto.NroDocumento))
            {
                return BadRequest(new { message = "El número de documento es obligatorio." });
            }

            if (clienteDto.FechaCreacion == DateTime.MinValue)
            {
                clienteDto.FechaCreacion = DateTime.Now;
            }

            try
            {
                var resultado = await _clienteService.Crear(clienteDto);
                return Ok(new { data = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el cliente.", details = ex.Message });
            }
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] ClienteDTO clienteDto)
        {
            try
            {
                var resultado = await _clienteService.Editar(clienteDto);
                return Ok(new { data = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al editar el cliente.", details = ex.Message });
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _clienteService.Eliminar(id);
                return Ok(new { data = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el cliente.", details = ex.Message });
            }
        }
    }
}

