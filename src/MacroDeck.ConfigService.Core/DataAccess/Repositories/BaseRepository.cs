using System.Linq.Expressions;
using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MacroDeck.ConfigService.Core.DataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T> 
    where T : BaseEntity
{
    private readonly ILogger _logger = Log.ForContext<T>();
    
    protected ConfigServiceContext Context { get; }

    protected BaseRepository(ConfigServiceContext context)
    {
        Context = context;
        Context.Database.EnsureCreated();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var all = await Context.Set<T>().AsNoTracking().ToArrayAsync();
        return all;
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var match = await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        return match;
    }

    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        var query = Context.Set<T>().AsNoTracking().Where(predicate);
        var matches = await query.ToArrayAsync();
        return matches;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        var exists = await Context.Set<T>().AsNoTracking().AnyAsync(predicate);
        return exists;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        var count = await Context.Set<T>().AsNoTracking().CountAsync(predicate);
        return count;
    }

    public async Task<int> CountAllAsync()
    {
        var count = await Context.Set<T>().AsNoTracking().CountAsync();
        return count;
    }

    public async Task InsertAsync(T obj)
    {
        await Context.Set<T>().AddAsync(obj);
        await SaveAsync();
    }

    public async Task UpdateAsync(T obj)
    {
        Context.Entry(obj).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await Context.Set<T>().FindAsync(id);
        if (existing != null)
        {
            Context.Set<T>().Remove(existing);
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error while updating entity {Type}", nameof(T));
        }
        finally
        {
            Context.ChangeTracker.Clear();
        }
    }
}