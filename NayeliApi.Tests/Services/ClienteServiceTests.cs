using Moq;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;
using NayeliApi.Core.Services;
using Xunit;

namespace NayeliApi.Tests.Services;

/// <summary>
/// Clase de pruebas unitarias para el servicio de clientes.
/// </summary>
public class ClienteServiceTests
{
    private readonly Mock<IRepository<Cliente>> _mockClienteRepository;
    private readonly ClienteService _clienteService;

    public ClienteServiceTests()
    {
        _mockClienteRepository = new Mock<IRepository<Cliente>>();
        _clienteService = new ClienteService(_mockClienteRepository.Object);
    }

    /// <summary>
    /// Se encarga de probar la creación de un cliente con datos válidos.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CrearCliente_ConDatosValidos_RetornaClienteCreado()
    {
        var cliente = new Cliente
        {
            Nombre = "Juan Pérez",
            FechaNacimiento = new DateTime(1990, 5, 15),
            Sexo = "Masculino",
            Ingresos = 50000
        };

        _mockClienteRepository
            .Setup(r => r.AddAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(cliente);

        _mockClienteRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var resultado = await _clienteService.CrearCliente(cliente);

        Assert.NotNull(resultado);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Juan Pérez", resultado.Nombre);
        _mockClienteRepository.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
        _mockClienteRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Se encarga de probar la creación de un cliente sin nombre, esperando que lance una excepción.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CrearCliente_SinNombre_LanzaExcepcion()
    {
        var cliente = new Cliente
        {
            Nombre = "",
            FechaNacimiento = new DateTime(1990, 5, 15),
            Sexo = "Masculino",
            Ingresos = 50000
        };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _clienteService.CrearCliente(cliente));
    }

    /// <summary>
    /// Se encarga de probar la obtención de un cliente por ID válido.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ObtenerClientePorId_ConIdValido_RetornaCliente()
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

        var resultado = await _clienteService.ObtenerClientePorId(clienteId);

        Assert.NotNull(resultado);
        Assert.Equal(clienteId, resultado.Id);
        Assert.Equal("Juan Pérez", resultado.Nombre);
    }
}
