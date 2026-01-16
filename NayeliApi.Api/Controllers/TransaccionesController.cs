using Microsoft.AspNetCore.Mvc;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Exceptions;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly ITransaccionService _transaccionService;

    public TransaccionesController(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }

    [HttpPost("deposito")]
    public async Task<ActionResult<Transaccion>> RealizarDeposito([FromBody] RealizarTransaccionDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaccion = await _transaccionService.RealizarDeposito(dto.NumeroCuenta, dto.Monto);
            return Ok(transaccion);
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

    [HttpPost("retiro")]
    public async Task<ActionResult<Transaccion>> RealizarRetiro([FromBody] RealizarTransaccionDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaccion = await _transaccionService.RealizarRetiro(dto.NumeroCuenta, dto.Monto);
            return Ok(transaccion);
        }
        catch (SaldoInsuficienteException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
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

    [HttpGet("{numeroCuenta}/historial")]
    public async Task<ActionResult<List<TransaccionResponseDto>>> ObtenerHistorial(string numeroCuenta)
    {
        try
        {
            var historial = await _transaccionService.ObtenerHistorial(numeroCuenta);
            return Ok(historial);
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

    [HttpGet("{numeroCuenta}/resumen")]
    public async Task<ActionResult<ResumenTransaccionesDto>> ObtenerResumen(string numeroCuenta)
    {
        try
        {
            var resumen = await _transaccionService.ObtenerResumen(numeroCuenta);
            return Ok(resumen);
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
