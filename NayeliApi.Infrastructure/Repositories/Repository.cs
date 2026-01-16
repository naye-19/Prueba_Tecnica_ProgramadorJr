using Microsoft.EntityFrameworkCore;
using NayeliApi.Core.Interfaces;
using NayeliApi.Infrastructure.Data;

namespace NayeliApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BancoDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(BancoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
