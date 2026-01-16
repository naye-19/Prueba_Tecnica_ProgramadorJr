using Microsoft.EntityFrameworkCore;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;
using NayeliApi.Infrastructure.Data;

namespace NayeliApi.Infrastructure.Repositories;

public class TransaccionRepository : Repository<Transaccion>, ITransaccionRepository
{
    private readonly BancoDbContext _context;

    public TransaccionRepository(BancoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Transaccion>> ObtenerPorCuentaId(Guid cuentaId)
    {
        return await _context.Transacciones
            .Where(t => t.CuentaBancariaId == cuentaId)
            .OrderByDescending(t => t.Fecha)
            .ToListAsync();
    }
}
