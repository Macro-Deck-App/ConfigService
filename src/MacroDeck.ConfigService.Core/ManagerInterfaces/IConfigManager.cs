using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Core.ManagerInterfaces;

public interface IConfigManager
{
    public Task<ActionResult<string>> GetConfigEncoded(string name);
    public Task<ActionResult<string>> GetConfigDecoded(string name);
    public Task<ActionResult> CreateUpdateConfig(string name, string configValue);
    public Task<ActionResult> UpdateAccessToken(string name, string newAccessToken);
    public Task<ActionResult<int>> GetConfigVersion(string name);
}