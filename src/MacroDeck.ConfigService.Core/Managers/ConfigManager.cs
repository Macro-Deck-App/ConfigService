using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.ConfigService.Core.DataTypes;
using MacroDeck.ConfigService.Core.Enums;
using MacroDeck.ConfigService.Core.Exceptions;
using MacroDeck.ConfigService.Core.Extensions;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using MacroDeck.ConfigService.Core.Results;
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

    public async Task<ActionResult<EncodedConfig>> GetConfigEncoded(string configName)
    {
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        
        return existingConfigEntity == null
            ? new NotFoundResult()
            : new EncodedConfig
            {
                Version = existingConfigEntity.Version,
                ConfigBase64 = existingConfigEntity.ConfigValue
            };
    }

    public async Task<string> GetConfigDecoded(string configName)
    {
        configName = SanitizeConfigName(configName);
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        if (existingConfigEntity == null)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }

        return existingConfigEntity.ConfigValue.DecodeBase64().TryWriteJsonIndented();
    }

    public async Task<ActionResult> UpdateAccessToken(string configName, string newAccessToken)
    {
        configName = SanitizeConfigName(configName);
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        if (existingConfigEntity == null)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }

        var hashedToken = PasswordHasher.HashPassword(newAccessToken);
        existingConfigEntity.AccessTokenHash = hashedToken.Hash;
        existingConfigEntity.AccessTokenSalt = hashedToken.Salt;

        await _serviceConfigRepository.UpdateAsync(existingConfigEntity);
        return new ConfigOkResult();
    }

    public async Task<ActionResult<int>> GetConfigVersion(string name)
    {
        var configVersion = await _serviceConfigRepository.GetVersion(name);
        if (!configVersion.HasValue)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }

        return configVersion.Value;
    }

    public async Task<List<string>> ListConfigs()
    {
        return await _serviceConfigRepository.ListConfigNames();
    }

    public async Task<ActionResult> DeleteConfig(string configName)
    {
        configName = SanitizeConfigName(configName);
        var existingConfig = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        if (existingConfig == null)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }

        await _serviceConfigRepository.DeleteAsync(existingConfig.Id);
        await _serviceConfigRepository.SaveAsync();
        return new ConfigOkResult();
    }

    public async Task<ActionResult> CreateConfig(string configName)
    {
        configName = SanitizeConfigName(configName);
        var exists = await _serviceConfigRepository.ExistsAsync(x => x.Name == configName);
        if (exists)
        {
            throw new ErrorCodeException(ErrorCode.ConfigAlreadyExists);
        }

        await _serviceConfigRepository.InsertAsync(new ServiceConfigEntity
        {
            Name = configName,
            Version = 1,
            ConfigValue = "{\n}".TrimJson().EncodeBase64()
        });
        await _serviceConfigRepository.SaveAsync();
        return new ConfigCreatedResult(configName, configName);
    }

    public async Task<ActionResult> UpdateConfig(string configName, string configValue)
    {
        configName = SanitizeConfigName(configName);
        var existingConfigEntity = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        if (existingConfigEntity == null)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }


        try
        {
            existingConfigEntity.ConfigValue = configValue.TrimJson();
        }
        catch
        {
            throw new ErrorCodeException(ErrorCode.ConfigInvalidJson);
        }
        finally
        {
            existingConfigEntity.ConfigValue = existingConfigEntity.ConfigValue.EncodeBase64();
        }
        
        await _serviceConfigRepository.UpdateAsync(existingConfigEntity);
        return new ConfigOkResult();
    }

    public async Task<string> GenerateAccessToken(string configName)
    {
        configName = SanitizeConfigName(configName);
        var existingConfig = await _serviceConfigRepository.FindAsync(x => x.Name == configName);
        if (existingConfig == null)
        {
            throw new ErrorCodeException(ErrorCode.ConfigNotFound);
        }
        var token = PasswordUtil.CreatePassword(64);
        var hashed = PasswordHasher.HashPassword(token);

        existingConfig.AccessTokenHash = hashed.Hash;
        existingConfig.AccessTokenSalt = hashed.Salt;

        await _serviceConfigRepository.UpdateAsync(existingConfig);

        return token;
    }

    private string SanitizeConfigName(string configName)
    {
        return configName.ToLower();
    }
}