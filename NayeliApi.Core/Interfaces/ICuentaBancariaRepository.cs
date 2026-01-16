using NayeliApi.Core.Entities;

namespace NayeliApi.Core.Interfaces;

public interface ICuentaBancariaRepository : IRepository<CuentaBancaria>
{
    Task<CuentaBancaria?> ObtenerPorNumeroCuenta(string numeroCuenta);
}
