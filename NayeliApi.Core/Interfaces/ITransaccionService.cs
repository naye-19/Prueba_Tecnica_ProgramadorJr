using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;

namespace NayeliApi.Core.Interfaces;

public interface ITransaccionService
{
    Task<Transaccion> RealizarDeposito(string numeroCuenta, decimal monto);
    Task<Transaccion> RealizarRetiro(string numeroCuenta, decimal monto);
    Task<List<TransaccionResponseDto>> ObtenerHistorial(string numeroCuenta);
    Task<ResumenTransaccionesDto> ObtenerResumen(string numeroCuenta);
}
