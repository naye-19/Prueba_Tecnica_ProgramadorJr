using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;

namespace NayeliApi.Core.Interfaces;

public interface IClienteService
{
    Task<Cliente> CrearCliente(Cliente cliente);
    Task<Cliente?> ObtenerClientePorId(Guid id);
}
