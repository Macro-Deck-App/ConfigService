using EvolveDb;
using MacroDeck.ConfigService.Core.Helper;
using Npgsql;
using Serilog;

namespace MacroDeck.ConfigService.Utils;

public class DatabaseMigrationUtil
{
    public static void MigrateDatabase()
    {
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = EnvironmentHelper.DatabaseHost,
                Port = EnvironmentHelper.DatabasePort,
                Database = EnvironmentHelper.DatabaseDatabase,
                Username = EnvironmentHelper.DatabaseUser,
                Password = EnvironmentHelper.DatabasePassword,
                Timeout = 120,
            };
            
            var connection = new NpgsqlConnection(connectionStringBuilder.ToString());
            var evolve = new Evolve(connection, Log.Information)
            {
                Locations = new[] { "Migrations" },
                IsEraseDisabled = true,
                Schemas = new []{ "evolve" }
            };

            evolve.Migrate();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Database migration failed");
            throw;
        }
    }
}