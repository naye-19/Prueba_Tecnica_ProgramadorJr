using NayeliApi.Core.Entities;

namespace NayeliApi.Core.Interfaces;

public interface ICuentaBancariaService
{
    Task<CuentaBancaria> CrearCuenta(Guid clienteId);
    Task<decimal> ConsultarSaldo(string numeroCuenta);
    Task<CuentaBancaria?> ObtenerCuentaPorNumero(string numeroCuenta);
}
