using Moq;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;
using NayeliApi.Core.Services;
using Xunit;

namespace NayeliApi.Tests.Services;

/// <summary>
/// Clase que se encarga de las pruebas unitarias para el servicio de cuentas bancarias.
/// </summary>
public class CuentaBancariaServiceTests
{
    private readonly Mock<ICuentaBancariaRepository> _mockCuentaRepository;
    private readonly Mock<IRepository<Cliente>> _mockClienteRepository;
    private readonly CuentaBancariaService _cuentaBancariaService;

    public CuentaBancariaServiceTests()
    {
        _mockCuentaRepository = new Mock<ICuentaBancariaRepository>();
        _mockClienteRepository = new Mock<IRepository<Cliente>>();
        _cuentaBancariaService = new CuentaBancariaService(
            _mockCuentaRepository.Object,
            _mockClienteRepository.Object);
    }

    /// <summary>
    /// Se encarga de probar el método CrearCuenta del servicio de cuentas bancarias.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CrearCuenta_ConClienteValido_RetornaCuentaConSaldoCero()
    {
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente
        {
            Id = clienteId,
            Nombre = "Juan Pérez",
            FechaNacimiento = new DateTime(1990, 5, 15),
            Sexo = "Masculino",
            Ingresos = 50000
        };

        _mockClienteRepository
            .Setup(r => r.GetByIdAsync(clienteId))
            .ReturnsAsync(cliente);

        _mockCuentaRepository
            .Setup(r => r.AddAsync(It.IsAny<CuentaBancaria>()))
            .ReturnsAsync((CuentaBancaria c) => c);

        _mockCuentaRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var resultado = await _cuentaBancariaService.CrearCuenta(clienteId);

        Assert.NotNull(resultado);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.NotEmpty(resultado.NumeroCuenta);
        Assert.StartsWith("BCCEHN", resultado.NumeroCuenta);
        Assert.Equal(0, resultado.SaldoActual);
        Assert.Equal(clienteId, resultado.ClienteId);
    }

    /// <summary>
    /// Se encarga de probar el método CrearCuenta del servicio de cuentas bancarias
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CrearCuenta_ConClienteInexistente_LanzaExcepcion()
    {
        var clienteId = Guid.NewGuid();

        _mockClienteRepository
            .Setup(r => r.GetByIdAsync(clienteId))
            .ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _cuentaBancariaService.CrearCuenta(clienteId));
    }

    /// <summary>
    /// Se encarga de probar el método ConsultarSaldo del servicio de cuentas bancarias.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ConsultarSaldo_ConCuentaExistente_RetornaSaldoCorrecto()
    {
        var cuenta = new CuentaBancaria
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = "BCCEHN123456789",
            ClienteId = Guid.NewGuid(),
            SaldoActual = 1000
        };

        _mockCuentaRepository
            .Setup(r => r.ObtenerPorNumeroCuenta("BCCEHN123456789"))
            .ReturnsAsync(cuenta);

        var saldo = await _cuentaBancariaService.ConsultarSaldo("BCCEHN123456789");

        Assert.Equal(1000, saldo);
    }
}
