using MacroDeck.ConfigService.Core.DataTypes;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Core.ManagerInterfaces;

public interface IConfigManager
{
    public Task<ActionResult<EncodedConfig>> GetConfigEncoded(string name);
    public Task<string> GetConfigDecoded(string name);
    public Task<ActionResult> UpdateAccessToken(string name, string newAccessToken);
    public Task<ActionResult<int>> GetConfigVersion(string name);
    public Task<List<string>> ListConfigs();
    public Task<ActionResult> DeleteConfig(string name);
    public Task<ActionResult> CreateConfig(string name);
    public Task<ActionResult> UpdateConfig(string name, string configValue);
    public Task<string> GenerateAccessToken(string name);
}