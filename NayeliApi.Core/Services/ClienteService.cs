using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;

namespace NayeliApi.Core.Services;

/// <summary>
/// Clase que implementa la lógica de negocio relacionada con los clientes.
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IRepository<Cliente> _clienteRepository;

    public ClienteService(IRepository<Cliente> clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Se encarga de la creación de un cliente.
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Cliente> CrearCliente(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nombre))
            throw new ArgumentException("El nombre es requerido");

        if (string.IsNullOrWhiteSpace(cliente.Sexo))
            throw new ArgumentException("El sexo es requerido");

        if (cliente.FechaNacimiento == default)
            throw new ArgumentException("La fecha de nacimiento es requerida");

        cliente.Id = Guid.NewGuid();

        await _clienteRepository.AddAsync(cliente);
        await _clienteRepository.SaveChangesAsync();

        return cliente;
    }

    /// <summary>
    /// Se encarga de obtener un cliente por su Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Cliente?> ObtenerClientePorId(Guid id)
    {
        return await _clienteRepository.GetByIdAsync(id);
    }
}
