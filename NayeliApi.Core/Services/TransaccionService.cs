using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Exceptions;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Core.Services;

/// <summary>
/// Clase que implementa la lógica de negocio relacionada con las transacciones.
/// </summary>
public class TransaccionService : ITransaccionService
{
    private readonly ITransaccionRepository _transaccionRepository;
    private readonly ICuentaBancariaService _cuentaBancariaService;
    private readonly ICuentaBancariaRepository _cuentaRepository;

    public TransaccionService(
        ITransaccionRepository transaccionRepository,
        ICuentaBancariaService cuentaBancariaService,
        ICuentaBancariaRepository cuentaRepository)
    {
        _transaccionRepository = transaccionRepository;
        _cuentaBancariaService = cuentaBancariaService;
        _cuentaRepository = cuentaRepository;
    }

    /// <summary>
    /// Se encarga de realizar un depósito en una cuenta bancaria.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <param name="monto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Transaccion> RealizarDeposito(string numeroCuenta, decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0");

        var cuenta = await _cuentaBancariaService.ObtenerCuentaPorNumero(numeroCuenta);
        if (cuenta == null)
            throw new ArgumentException("La cuenta no existe");

        cuenta.SaldoActual += monto;

        var transaccion = new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaBancariaId = cuenta.Id,
            Tipo = TipoTransaccion.Deposito,
            Monto = monto,
            Fecha = DateTime.UtcNow,
            SaldoDespues = cuenta.SaldoActual
        };

        await _transaccionRepository.AddAsync(transaccion);
        await _cuentaRepository.UpdateAsync(cuenta);
        await _transaccionRepository.SaveChangesAsync();

        return transaccion;
    }

    /// <summary>
    /// Se encarga de realizar un retiro en una cuenta bancaria.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <param name="monto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="SaldoInsuficienteException"></exception>
    public async Task<Transaccion> RealizarRetiro(string numeroCuenta, decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0");

        var cuenta = await _cuentaBancariaService.ObtenerCuentaPorNumero(numeroCuenta);
        if (cuenta == null)
            throw new ArgumentException("La cuenta no existe");

        if (cuenta.SaldoActual < monto)
            throw new SaldoInsuficienteException($"Saldo insuficiente. Saldo actual: {cuenta.SaldoActual}, Monto solicitado: {monto}");

        cuenta.SaldoActual -= monto;

        var transaccion = new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaBancariaId = cuenta.Id,
            Tipo = TipoTransaccion.Retiro,
            Monto = monto,
            Fecha = DateTime.UtcNow,
            SaldoDespues = cuenta.SaldoActual
        };

        await _transaccionRepository.AddAsync(transaccion);
        await _cuentaRepository.UpdateAsync(cuenta);
        await _transaccionRepository.SaveChangesAsync();

        return transaccion;
    }

    /// <summary>
    /// Se encarga de obtener el historial de transacciones de una cuenta bancaria.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<List<TransaccionResponseDto>> ObtenerHistorial(string numeroCuenta)
    {
        var cuenta = await _cuentaBancariaService.ObtenerCuentaPorNumero(numeroCuenta);
        if (cuenta == null)
            throw new ArgumentException("La cuenta no existe");

        var transacciones = await _transaccionRepository.ObtenerPorCuentaId(cuenta.Id);

        return transacciones.Select(t => new TransaccionResponseDto
        {
            Id = t.Id,
            Tipo = t.Tipo,
            TipoDescripcion = t.Tipo == TipoTransaccion.Deposito ? "Depósito" : "Retiro",
            Monto = t.Monto,
            Fecha = t.Fecha,
            SaldoDespues = t.SaldoDespues
        }).ToList();
    }

    /// <summary>
    /// Se encarga de obtener un resumen de las transacciones de una cuenta bancaria.
    /// </summary>
    /// <param name="numeroCuenta"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<ResumenTransaccionesDto> ObtenerResumen(string numeroCuenta)
    {
        var cuenta = await _cuentaBancariaService.ObtenerCuentaPorNumero(numeroCuenta);
        if (cuenta == null)
            throw new ArgumentException("La cuenta no existe");

        var transacciones = await _transaccionRepository.ObtenerPorCuentaId(cuenta.Id);

        var totalDepositos = transacciones
            .Where(t => t.Tipo == TipoTransaccion.Deposito)
            .Sum(t => t.Monto);

        var totalRetiros = transacciones
            .Where(t => t.Tipo == TipoTransaccion.Retiro)
            .Sum(t => t.Monto);

        return new ResumenTransaccionesDto
        {
            TotalDepositos = totalDepositos,
            TotalRetiros = totalRetiros,
            SaldoFinal = cuenta.SaldoActual,
            Transacciones = transacciones
                .OrderBy(t => t.Fecha)
                .Select(t => new TransaccionResponseDto
                {
                    Id = t.Id,
                    Tipo = t.Tipo,
                    TipoDescripcion = t.Tipo == TipoTransaccion.Deposito ? "Depósito" : "Retiro",
                    Monto = t.Monto,
                    Fecha = t.Fecha,
                    SaldoDespues = t.SaldoDespues
                }).ToList()
        };
    }
}
