using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Core.Results;

public class ConfigCreatedResult : CreatedResult
{
    public bool Success => true;
    
    public ConfigCreatedResult(string location, object? value) 
        : base(location, value)
    {
    }

    public ConfigCreatedResult(Uri location, object? value) 
        : base(location, value)
    {
    }
}