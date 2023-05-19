using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.ConfigService.Core.Extensions;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using MacroDeck.ConfigService.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Core.Managers;

public class ConfigManager : IConfigManager
{
    private readonly IServiceConfigRepository _serviceConfigRepository;

    public ConfigManager(IServiceConfigRepository serviceConfigRepository)
    {
        _serviceConfigRepository = serviceConfigRepository;
    }

    public async Task<ActionResult<string>> GetConfigEncoded(string name)
    {
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == name);
        
        return existingConfigEntity == null
            ? new NotFoundResult()
            : existingConfigEntity.ConfigValue;
    }

    public async Task<ActionResult<string>> GetConfigDecoded(string name)
    {
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == name);
        if (existingConfigEntity == null)
        {
            return new NotFoundResult();
        }

        return existingConfigEntity.ConfigValue.DecodeBase64().TryWriteJsonIndented();
    }

    public async Task<ActionResult> CreateUpdateConfig(string name, string configValue)
    {
        var jsonBase64 = configValue.TryTrimJson().EncodeBase64();
        var exists =
            await _serviceConfigRepository.ExistsAsync(x =>
                x.Name.ToLower() == name.ToLower());
        if (exists)
        {
            await UpdateConfig(name, jsonBase64);
            return new OkResult();
        }

        await CreateConfig(name, jsonBase64);
        return new CreatedResult(name, string.Empty);
    }

    public async Task<ActionResult> UpdateAccessToken(string name, string newAccessToken)
    {
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == name);
        if (existingConfigEntity == null)
        {
            return new NotFoundResult();
        }

        var hashedToken = PasswordHasher.HashPassword(newAccessToken);
        existingConfigEntity.AccessTokenHash = hashedToken.Hash;
        existingConfigEntity.AccessTokenSalt = hashedToken.Salt;

        await _serviceConfigRepository.UpdateAsync(existingConfigEntity);
        return new OkResult();
    }

    public async Task<ActionResult<int>> GetConfigVersion(string name)
    {
        var configVersion = await _serviceConfigRepository.GetVersion(name);
        if (!configVersion.HasValue)
        {
            return new NotFoundResult();
        }

        return configVersion.Value;
    }

    private async Task CreateConfig(string name, string base64)
    {
        var configEntity = new ServiceConfigEntity
        {
            Name = name,
            ConfigValue = base64
        };

        await _serviceConfigRepository.InsertAsync(configEntity);
    }

    private async Task UpdateConfig(string name, string base64)
    {
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == name);
        if (existingConfigEntity == null)
        {
            return;
        }
        
        existingConfigEntity.ConfigValue = base64;
        await _serviceConfigRepository.UpdateAsync(existingConfigEntity);
    }
}