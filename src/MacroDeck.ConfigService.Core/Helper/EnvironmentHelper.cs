using Serilog;

namespace MacroDeck.ConfigService.Core.Helper;

public class EnvironmentHelper
{
    private static readonly ILogger Logger = Log.ForContext(typeof(EnvironmentHelper));
    
    private const string CurrentEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
    private const string AdminAccessTokenVariable = "ADMIN_ACCESS_TOKEN";
    private const string HostingPortVariable = "HOSTING_PORT";
    private const string DatabaseHostVariable = "DB_HOST";
    private const string DatabasePortVariable = "DB_PORT";
    private const string DatabaseUserVariable = "DB_USER";
    private const string DatabasePasswordVariable = "DB_PASSWORD";
    private const string DatabaseDatabaseVariable = "DB_DATABASE";
    private const string JwtSecretVariable = "JWT_SECRET";
    private const string JwtIssuerVariable = "JWT_ISSUER";
    private const string JwtAudienceVariable = "JWT_AUDIENCE";

    public static bool IsLocalDevelopment => IsCurrentEnvironment("LocalDevelopment");
    public static bool IsTesting => IsCurrentEnvironment("Testing");
    public static bool IsProduction => IsCurrentEnvironment("Production");
    public static bool IsTestingOrProduction => IsTesting || IsProduction;
    public static string AdminAccessToken => GetStringFromEnvironmentVariable(AdminAccessTokenVariable);
    public static int HostingPort => GetIntFromEnvironmentVariable(HostingPortVariable);
    public static string DatabaseHost => GetStringFromEnvironmentVariable(DatabaseHostVariable);
    public static int DatabasePort => GetIntFromEnvironmentVariable(DatabasePortVariable, 5432);
    public static string DatabaseUser => GetStringFromEnvironmentVariable(DatabaseUserVariable);
    public static string DatabasePassword => GetStringFromEnvironmentVariable(DatabasePasswordVariable);
    public static string DatabaseDatabase => GetStringFromEnvironmentVariable(DatabaseDatabaseVariable);
    public static string JwtSecret => GetStringFromEnvironmentVariable(JwtSecretVariable);
    public static string JwtIssuer => GetStringFromEnvironmentVariable(JwtIssuerVariable);
    public static string JwtAudience => GetStringFromEnvironmentVariable(JwtAudienceVariable);

    private static bool IsCurrentEnvironment(string environmentToCompare)
    {
        return GetStringFromEnvironmentVariable(CurrentEnvironmentVariable)
            .Equals(environmentToCompare, StringComparison.InvariantCultureIgnoreCase);
    }
    
    private static string GetStringFromEnvironmentVariable(string environmentVariable, string? fallback = null)
    {
        var result = Environment.GetEnvironmentVariable(environmentVariable);
        if (string.IsNullOrWhiteSpace(result))
        {
            Logger.Fatal("Environment variable {Variable} was empty", environmentVariable);
        }

        return result ?? fallback ?? string.Empty;
    }

    private static int GetIntFromEnvironmentVariable(string environmentVariable, int? fallback = null)
    {
        var variableValue = GetStringFromEnvironmentVariable(environmentVariable);
        if (int.TryParse(variableValue, out var result))
        {
            return result;
        }
        
        Logger.Fatal("Cannot parse environment variable {Variable} to integer", environmentVariable);

        return fallback ?? -1;
    }
}