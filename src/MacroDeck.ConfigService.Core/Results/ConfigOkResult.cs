using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Core.Results;

public class ConfigOkResult : OkResult
{
    public bool Success => true;
}