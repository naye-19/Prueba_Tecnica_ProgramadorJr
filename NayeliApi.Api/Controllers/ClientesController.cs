using Microsoft.AspNetCore.Mvc;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente([FromBody] CrearClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                FechaNacimiento = dto.FechaNacimiento,
                Sexo = dto.Sexo,
                Ingresos = dto.Ingresos
            };

            var clienteCreado = await _clienteService.CrearCliente(cliente);
            return CreatedAtAction(nameof(ObtenerCliente), new { id = clienteCreado.Id }, clienteCreado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> ObtenerCliente(Guid id)
    {
        try
        {
            var cliente = await _clienteService.ObtenerClientePorId(id);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }
}
