using MacroDeck.ConfigService.Core;
using MacroDeck.ConfigService.Core.Authorization;
using MacroDeck.ConfigService.Core.DataTypes;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<string>>> ListConfigs()
    {
        return await _configManager.ListConfigs();
    }

    [HttpGet("{name}")]
    [Authorize]
    public async Task<ActionResult<string>> GetConfigDecoded(string name)
    {
        return await _configManager.GetConfigDecoded(name);
    }

    [HttpPost("{name}")]
    [Authorize]
    public async Task<IActionResult> CreateConfig(string name)
    {
        return await _configManager.CreateConfig(name);
    }
    
    [HttpPut("{name}")]
    [Authorize]
    public async Task<IActionResult> UpdateConfig(string name)
    {
        using var reader = new StreamReader(Request.Body);
        var configValue = await reader.ReadToEndAsync();
        
        return await _configManager.UpdateConfig(name, configValue);
    }

    [HttpDelete("{name}")]
    [Authorize]
    public async Task DeleteConfig(string name)
    {
        await _configManager.DeleteConfig(name);
    }

    [HttpPost("{name}/token/generate")]
    [Authorize]
    public async Task<string> GenerateAccessToken(string name)
    {
        return await _configManager.GenerateAccessToken(name);
    }
}