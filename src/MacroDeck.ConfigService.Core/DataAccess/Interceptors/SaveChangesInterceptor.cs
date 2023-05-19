using MacroDeck.ConfigService.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MacroDeck.ConfigService.Core.DataAccess.Interceptors;

public class SaveChangesInterceptor : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context?.ChangeTracker.Entries() is null)
        {
            return ValueTask.FromResult(result);
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedTimestamp = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        baseEntity.UpdatedTimestamp = DateTime.Now;
                        break;
                }
            }

            if (entry.Entity is ServiceConfigEntity serviceConfigEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        serviceConfigEntity.Version = 1;
                        break;
                    case EntityState.Modified:
                        serviceConfigEntity.Version++;
                        break;
                }
            }
        }

        return ValueTask.FromResult(result);
    }
}