using Microsoft.EntityFrameworkCore;
using NayeliApi.Core.Entities;
using NayeliApi.Core.Interfaces;
using NayeliApi.Infrastructure.Data;

namespace NayeliApi.Infrastructure.Repositories;

public class CuentaBancariaRepository : Repository<CuentaBancaria>, ICuentaBancariaRepository
{
    private readonly BancoDbContext _context;

    public CuentaBancariaRepository(BancoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CuentaBancaria?> ObtenerPorNumeroCuenta(string numeroCuenta)
    {
        return await _context.CuentasBancarias
            .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
    }
}
