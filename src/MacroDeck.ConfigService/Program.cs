using MacroDeck.ConfigService.Core.Helper;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using MacroDeck.ConfigService.StartupConfig;
using MacroDeck.ConfigService.Utils;
using Serilog;

namespace MacroDeck.ConfigService;

public static class Program
{
    public static async Task Main(string[] args) 
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        
        DatabaseMigrationUtil.MigrateDatabase();
        
        var app = Host.CreateDefaultBuilder(args)
            .ConfigureSerilog()
            .ConfigureWebHostDefaults(hostBuilder =>
            {
                hostBuilder.UseStartup<Startup>();
                hostBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(EnvironmentHelper.HostingPort);
                });
            }).Build();
        
        await app.Services.GetRequiredService<IUserManager>().CreateDefaultUser();
        await app.RunAsync();
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Logger.Fatal(e.ExceptionObject as Exception,
            "Unhandled exception {Terminating}",
            e.IsTerminating
                ? "Terminating"
                : "Not terminating");
    }
}