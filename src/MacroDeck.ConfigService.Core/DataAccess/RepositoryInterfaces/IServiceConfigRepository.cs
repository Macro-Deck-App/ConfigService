using MacroDeck.ConfigService.Core.DataAccess.Entities;

namespace MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;

public interface IServiceConfigRepository : IBaseRepository<ServiceConfigEntity>
{
    public Task<bool> VerifyConfigToken(string configName, string accessToken);
    public Task<int?> GetVersion(string configName);
    public Task<List<string>> ListConfigNames();
}