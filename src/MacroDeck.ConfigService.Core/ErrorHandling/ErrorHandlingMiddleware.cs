using System.Text.Json;
using MacroDeck.ConfigService.Core.Exceptions;
using MacroDeck.ConfigService.Core.Extensions;
using MacroDeck.ConfigService.Core.Helper;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MacroDeck.ConfigService.Core.ErrorHandling;

public class ErrorHandlingMiddleware
{
    private readonly ILogger _logger = Log.ForContext<ErrorHandlingMiddleware>();
    
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case UnauthorizedException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(string.Empty);
                return;
            case ForbiddenException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(string.Empty);
                return;
            case ErrorCodeException errorCodeException:
                await HandleErrorCodeException(context, errorCodeException);
                break;
            default:
                await HandleUnhandledException(context, exception);
                break;
        }
    }

    private async Task HandleErrorCodeException(HttpContext context, ErrorCodeException errorCodeException)
    {
        context.Response.StatusCode = errorCodeException.ErrorCode.GetStatusCode() ?? StatusCodes.Status400BadRequest;

        object errorMessage = new
        {
            Success = false,
            errorCodeException.ErrorCode,
            Message = errorCodeException.ErrorCode.GetDescription()
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage));
    }

    private async Task HandleUnhandledException(HttpContext context, Exception exception)
    {
        _logger.Fatal(exception, "Unhandled error on request {Request}", context.Request.Path);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        if (EnvironmentHelper.IsProduction)
        {
            await context.Response.WriteAsync(string.Empty);
            return;
        }
        
        object errorMessage = new
        {
            exception.Message,
            exception.StackTrace
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage));
    }
}