using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MacroDeck.ConfigService.Core.Authorization;

public class AccessTokenFilter : IAsyncAuthorizationFilter
{
    private readonly IServiceConfigRepository _serviceConfigRepository;

    public AccessTokenFilter(IServiceConfigRepository serviceConfigRepository)
    {
        _serviceConfigRepository = serviceConfigRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(Constants.AccessTokenHeader, out var accessToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        context.HttpContext.Request.Headers.Remove(Constants.AccessTokenHeader);
        
        if (accessToken.Count > 1)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(Constants.ConfigNameHeader, out var configName))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        var valid = await _serviceConfigRepository.VerifyConfigToken(configName.ToString(), accessToken.ToString());
        if (!valid)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}