using MacroDeck.ConfigService.Core.Helper;

namespace MacroDeck.ConfigService.StartupConfig;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        if (EnvironmentHelper.IsProduction)
        {
            return;
        }
        
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}