using MacroDeck.ConfigService.Core.DataAccess.Entities;

namespace MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    public Task UpdateLastLogin(int userId);
}