using MacroDeck.ConfigService.Core.Authorization;
using MacroDeck.ConfigService.Core.DataAccess;
using MacroDeck.ConfigService.StartupConfig;

namespace MacroDeck.ConfigService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ConfigServiceContext>();
        services.AddSwagger();
        services.RegisterAutoMapperProfiles();
        services.RegisterRestInterfaceControllers();
        services.AddScoped<AccessTokenFilter>();
        services.AddScoped<AdminTokenFilter>();
        services.RegisterClassesEndsWithAsScoped("Repository");
        services.RegisterClassesEndsWithAsScoped("Manager");
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAny");
        //app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseFileServer();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.ConfigureSwagger();
    }
}