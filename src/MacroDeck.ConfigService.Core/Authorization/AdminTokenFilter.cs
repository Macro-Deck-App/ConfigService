using MacroDeck.ConfigService.Core.Extensions;
using MacroDeck.ConfigService.Core.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MacroDeck.ConfigService.Core.Authorization;

public class AdminTokenFilter : IAuthorizationFilter
{
    private readonly ILogger _logger = Log.ForContext<AdminTokenFilter>();
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(Constants.AdminTokenHeader, out var adminToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Request.Headers.Remove(Constants.AdminTokenHeader);

        if (adminToken.Count > 1)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (VerifyToken(adminToken.ToString()))
        {
            return;
        }
        
        _logger.Fatal("Failed login using admin token {AdminToken}", adminToken);
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
    }
    
    private static bool VerifyToken(string extractedToken)
    {
        var adminTokenBase64 = Convert.FromBase64String(extractedToken.EncodeBase64());
        var validTokenBase64 = Convert.FromBase64String(EnvironmentHelper.AdminAccessToken.EncodeBase64());

        if (adminTokenBase64.Length != validTokenBase64.Length)
        {
            return false;
        }
        
        var differences = 0;

        for (var i = 0; i < adminTokenBase64.Length; i++)
        {
            differences |= (adminTokenBase64[i] ^ validTokenBase64[i]);
        }

        return differences == 0;
    }
}