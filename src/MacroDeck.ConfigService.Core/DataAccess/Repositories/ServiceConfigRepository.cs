using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.ConfigService.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.ConfigService.Core.DataAccess.Repositories;

public class ServiceConfigRepository : BaseRepository<ServiceConfigEntity>, IServiceConfigRepository
{
    public ServiceConfigRepository(ConfigServiceContext context)
        : base(context)
    {
    }

    public async Task<bool> VerifyConfigToken(string configName, string accessToken)
    {
        var serviceConfig = await Context.Set<ServiceConfigEntity>()
            .SingleOrDefaultAsync(x => x.Name.ToLower() == configName.ToLower());

        return serviceConfig != null &&
               PasswordHasher.VerifyPassword(accessToken, serviceConfig.AccessTokenHash, serviceConfig.AccessTokenSalt);
    }

    public async Task<int?> GetVersion(string configName)
    {
        return await Context.Set<ServiceConfigEntity>()
            .Where(x => x.Name == configName)
            .Select(x => x.Version)
            .SingleOrDefaultAsync();
    }
}