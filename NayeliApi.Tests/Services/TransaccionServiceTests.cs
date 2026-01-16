using Moq;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Exceptions;
using NayeliApi.Core.Interfaces;
using NayeliApi.Core.Services;
using Xunit;

namespace NayeliApi.Tests.Services;

/// <summary>
/// Clase que se encarga de las pruebas unitarias para el servicio de transacciones.
/// </summary>
public class TransaccionServiceTests
{
    private readonly Mock<ITransaccionRepository> _mockTransaccionRepository;
    private readonly Mock<ICuentaBancariaService> _mockCuentaBancariaService;
    private readonly Mock<ICuentaBancariaRepository> _mockCuentaRepository;
    private readonly TransaccionService _transaccionService;

    public TransaccionServiceTests()
    {
        _mockTransaccionRepository = new Mock<ITransaccionRepository>();
        _mockCuentaBancariaService = new Mock<ICuentaBancariaService>();
        _mockCuentaRepository = new Mock<ICuentaBancariaRepository>();
        _transaccionService = new TransaccionService(
            _mockTransaccionRepository.Object,
            _mockCuentaBancariaService.Object,
            _mockCuentaRepository.Object);
    }

    /// <summary>
    /// Se encarga de probar que al realizar un depósito con un monto válido.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RealizarDeposito_ConMontoValido_IncrementaSaldo()
    {
        var cuenta = new CuentaBancaria
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 1000
        };

        _mockCuentaBancariaService
            .Setup(s => s.ObtenerCuentaPorNumero("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        _mockTransaccionRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaccion>()))
            .ReturnsAsync((Transaccion t) => t);

        _mockTransaccionRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var resultado = await _transaccionService.RealizarDeposito("BCCEHN123456789", 500);

        Assert.NotNull(resultado);
        Assert.Equal(TipoTransaccion.Deposito, resultado.Tipo);
        Assert.Equal(500, resultado.Monto);
        Assert.Equal(1500, resultado.SaldoDespues);
        Assert.Equal(1500, cuenta.SaldoActual);
    }

    /// <summary>
    /// Se encarga de probar que al realizar un retiro con fondos suficientes.
    /// </summary>
    /// <returns></returns>

    [Fact]
    public async Task RealizarRetiro_ConFondosSuficientes_DecrementaSaldo()
    {
        var cuenta = new CuentaBancaria
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 1000
        };

        _mockCuentaBancariaService
            .Setup(s => s.ObtenerCuentaPorNumero("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        _mockTransaccionRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaccion>()))
            .ReturnsAsync((Transaccion t) => t);

        _mockTransaccionRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var resultado = await _transaccionService.RealizarRetiro("BCCEHN123456789", 300);

        Assert.NotNull(resultado);
        Assert.Equal(TipoTransaccion.Retiro, resultado.Tipo);
        Assert.Equal(300, resultado.Monto);
        Assert.Equal(700, resultado.SaldoDespues);
        Assert.Equal(700, cuenta.SaldoActual);
    }

    /// <summary>
    /// Se encarga de probar que al realizar un retiro sin fondos suficientes lanza una excepción.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RealizarRetiro_SinFondosSuficientes_LanzaExcepcion()
    {
        var cuenta = new CuentaBancaria
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 100
        };

        _mockCuentaBancariaService
            .Setup(s => s.ObtenerCuentaPorNumero("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        await Assert.ThrowsAsync<SaldoInsuficienteException>(async () =>
            await _transaccionService.RealizarRetiro("BCCEHN123456789", 500));
    }

    /// <summary>
    /// Se encarga de probar que al obtener el historial de transacciones se retorna ordenado por fecha.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ObtenerHistorial_ConTransacciones_RetornaOrdenadoPorFecha()
    {
        var cuentaId = Guid.NewGuid();
        var cuenta = new CuentaBancaria
        {
            Id = cuentaId,
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 1000
        };

        var transacciones = new List<Transaccion>
        {
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaBancariaId = cuentaId,
                Tipo = TipoTransaccion.Deposito,
                Monto = 500,
                Fecha = DateTime.UtcNow.AddDays(-1),
                SaldoDespues = 1000
            },
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaBancariaId = cuentaId,
                Tipo = TipoTransaccion.Deposito,
                Monto = 500,
                Fecha = DateTime.UtcNow.AddDays(-2),
                SaldoDespues = 500
            }
        };

        _mockCuentaBancariaService
            .Setup(s => s.ObtenerCuentaPorNumero("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        _mockTransaccionRepository
            .Setup(r => r.ObtenerPorCuentaId(cuentaId))
            .ReturnsAsync(transacciones);

        var historial = await _transaccionService.ObtenerHistorial("BCCEHN123456789");

        Assert.Equal(2, historial.Count);
    }

    /// <summary>
    /// Se encarga de probar que al obtener el resumen de transacciones se calculan los totales correctamente.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ObtenerResumen_CalculaTotalesCorrectamente()
    {
        var cuentaId = Guid.NewGuid();
        var cuenta = new CuentaBancaria
        {
            Id = cuentaId,
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 700
        };

        var transacciones = new List<Transaccion>
        {
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaBancariaId = cuentaId,
                Tipo = TipoTransaccion.Deposito,
                Monto = 500,
                Fecha = DateTime.UtcNow.AddDays(-3),
                SaldoDespues = 500
            },
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaBancariaId = cuentaId,
                Tipo = TipoTransaccion.Deposito,
                Monto = 500,
                Fecha = DateTime.UtcNow.AddDays(-2),
                SaldoDespues = 1000
            },
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaBancariaId = cuentaId,
                Tipo = TipoTransaccion.Retiro,
                Monto = 300,
                Fecha = DateTime.UtcNow.AddDays(-1),
                SaldoDespues = 700
            }
        };

        _mockCuentaBancariaService
            .Setup(s => s.ObtenerCuentaPorNumero("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        _mockTransaccionRepository
            .Setup(r => r.ObtenerPorCuentaId(cuentaId))
            .ReturnsAsync(transacciones);

        var resumen = await _transaccionService.ObtenerResumen("BCCEHN123456789");

        Assert.Equal(1000, resumen.TotalDepositos);
        Assert.Equal(300, resumen.TotalRetiros);
        Assert.Equal(700, resumen.SaldoFinal);
        Assert.Equal(3, resumen.Transacciones.Count);
    }
}
