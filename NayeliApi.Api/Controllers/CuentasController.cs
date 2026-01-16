using Microsoft.AspNetCore.Mvc;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly ICuentaBancariaService _cuentaBancariaService;

    public CuentasController(ICuentaBancariaService cuentaBancariaService)
    {
        _cuentaBancariaService = cuentaBancariaService;
    }

    [HttpPost]
    public async Task<ActionResult<CuentaBancariaResponseDto>> CrearCuenta([FromBody] CrearCuentaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cuenta = await _cuentaBancariaService.CrearCuenta(dto.ClienteId);

            var response = new CuentaBancariaResponseDto
            {
                Id = cuenta.Id,
                NumeroCuenta = cuenta.NumeroCuenta,
                ClienteId = cuenta.ClienteId,
                NombreCliente = cuenta.Cliente?.Nombre ?? string.Empty,
                SaldoActual = cuenta.SaldoActual
            };

            return CreatedAtAction(nameof(ConsultarSaldo), new { numeroCuenta = cuenta.NumeroCuenta }, response);
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

    [HttpGet("{numeroCuenta}/saldo")]
    public async Task<ActionResult<object>> ConsultarSaldo(string numeroCuenta)
    {
        try
        {
            var saldo = await _cuentaBancariaService.ConsultarSaldo(numeroCuenta);
            return Ok(new { numeroCuenta, saldo });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }
}
