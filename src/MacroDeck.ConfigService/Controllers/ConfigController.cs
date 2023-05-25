using MacroDeck.ConfigService.Core;
using MacroDeck.ConfigService.Core.Authorization;
using MacroDeck.ConfigService.Core.DataTypes;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Controllers;

[Route("config")]
public class ConfigController : ControllerBase
{
    private readonly IConfigManager _configManager;

    public ConfigController(IConfigManager configManager)
    {
        _configManager = configManager;
    }

    [HttpGet("encoded")]
    [ServiceFilter(typeof(AccessTokenFilter))]
    public async Task<ActionResult<EncodedConfig>> GetConfigEncoded()
    {
        if (!HttpContext.Request.Headers.TryGetValue(Constants.ConfigNameHeader, out var name))
        {
            return NotFound();
        }
        return await _configManager.GetConfigEncoded(name.ToString());
    }

    [HttpGet("{name}")]
    [ServiceFilter(typeof(AdminTokenFilter))]
    public async Task<ActionResult<string>> GetConfigDecoded(string name)
    {
        return await _configManager.GetConfigDecoded(name);
    }
    
    [HttpGet("version")]
    [ServiceFilter(typeof(AccessTokenFilter))]
    public async Task<ActionResult<int>> GetConfigVersion()
    {
        if (!HttpContext.Request.Headers.TryGetValue(Constants.ConfigNameHeader, out var name))
        {
            return NotFound();
        }

        return await _configManager.GetConfigVersion(name.ToString());
    }

    [HttpPost("{name}")]
    [ServiceFilter(typeof(AdminTokenFilter))]
    public async Task<IActionResult> AddUpdateConfig(string name)
    {
        using var reader = new StreamReader(Request.Body);
        var configValue = await reader.ReadToEndAsync();
        
        return await _configManager.CreateUpdateConfig(name, configValue);
    }

    [HttpPost("{name}/settoken")]
    [ServiceFilter(typeof(AdminTokenFilter))]
    public async Task<IActionResult> UpdateConfigAccessToken(string name, [FromBody] string newToken)
    {
        return await _configManager.UpdateAccessToken(name, newToken);
    }
}