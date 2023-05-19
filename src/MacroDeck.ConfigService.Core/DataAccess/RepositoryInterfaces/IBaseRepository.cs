using System.Linq.Expressions;
using MacroDeck.ConfigService.Core.DataAccess.Entities;

namespace MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;

public interface IBaseRepository<T> 
    where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    public Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    public Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    public Task<int> CountAllAsync();
    public Task InsertAsync(T obj);
    public Task UpdateAsync(T obj);
    public Task DeleteAsync(int id);
    public Task SaveAsync();
}