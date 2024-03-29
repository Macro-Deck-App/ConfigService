namespace MacroDeck.ConfigService.StartupConfig;

public static class AutoMapperConfig
{
    public static void RegisterAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup).Assembly);
    }
}