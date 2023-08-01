using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.ConfigService.Core.DataAccess.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(ConfigServiceContext context) 
        : base(context)
    {
    }

    public async Task UpdateLastLogin(int userId)
    {
        await Context.Set<UserEntity>().AsNoTracking()
            .ExecuteUpdateAsync(x => x.SetProperty(u => u.LastLogin, DateTime.Now));
    }
}