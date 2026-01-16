using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Core.Services;

/// <summary>
/// Clase que implementa la lógica de negocio relacionada con las cuentas bancarias.
/// </summary>
public class CuentaBancariaService : ICuentaBancariaService
{
    private readonly ICuentaBancariaRepository _cuentaRepository;
    private readonly IRepository<Cliente> _clienteRepository;

    public CuentaBancariaService(
        ICuentaBancariaRepository cuentaRepository,
        IRepository<Cliente> clienteRepository)
    {
        _cuentaRepository = cuentaRepository;
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Se encarga de crear una nueva cuenta bancaria para un cliente existente.
    /// </summary>
    /// <param name="clienteId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<CuentaBancaria> CrearCuenta(Guid clienteId)
    {
        var cliente = await _clienteRepository.GetByIdAsync(clienteId);
        if (cliente == null)
            throw new ArgumentException("El cliente no existe");

        var cuenta = new CuentaBancaria
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            NumeroCuenta = GenerarNumeroCuenta(),
            SaldoActual = 0
        };

        await _cuentaRepository.AddAsync(cuenta);
        await _cuentaRepository.SaveChangesAsync();

        return cuenta;
    }

    /// <summary>
    /// Se encarga de consultar el saldo actual de una cuenta bancaria dada su número de cuenta.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<decimal> ConsultarSaldo(string numeroCuenta)
    {
        var cuenta = await ObtenerCuentaPorNumero(numeroCuenta);
        if (cuenta == null)
            throw new ArgumentException("La cuenta no existe");

        return cuenta.SaldoActual;
    }

    /// <summary>
    /// Se encarga de obtener una cuenta bancaria por su número de cuenta.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <returns></returns>
    public async Task<CuentaBancaria?> ObtenerCuentaPorNumero(string numeroCuenta)
    {
        return await _cuentaRepository.ObtenerPorNumeroCuenta(numeroCuenta);
    }

    /// <summary>
    /// Se encarga de generar un número de cuenta único.
    /// </summary>
    /// <returns></returns>
    private string GenerarNumeroCuenta()
    {
        return "BCCEHN" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
    }
}
