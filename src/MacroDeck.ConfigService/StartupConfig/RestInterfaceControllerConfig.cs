using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MacroDeck.ConfigService.StartupConfig;

public static class RestInterfaceControllerConfig
{
    public static void RegisterRestInterfaceControllers(this IServiceCollection services)
    {
        var assembly = typeof(Startup).Assembly;
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "AllowAny",
                    builder =>
                        builder.SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .Build());
            });
        services.AddMvc();
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opt.JsonSerializerOptions.Converters.Add(enumConverter);
                opt.JsonSerializerOptions.AllowTrailingCommas = true;
            })
            .PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
    }
}