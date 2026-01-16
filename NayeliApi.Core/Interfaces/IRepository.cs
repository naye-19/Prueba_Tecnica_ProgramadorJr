namespace NayeliApi.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task SaveChangesAsync();
}
