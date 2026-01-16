using NayeliApi.Core.Entities;

namespace NayeliApi.Core.Interfaces;

public interface ITransaccionRepository : IRepository<Transaccion>
{
    Task<List<Transaccion>> ObtenerPorCuentaId(Guid cuentaId);
}
