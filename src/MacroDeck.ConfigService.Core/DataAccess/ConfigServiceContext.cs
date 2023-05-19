using System.Reflection;
using MacroDeck.ConfigService.Core.DataAccess.Interceptors;
using MacroDeck.ConfigService.Core.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog;

namespace MacroDeck.ConfigService.Core.DataAccess;

public class ConfigServiceContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
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

        var loggerFactory = new LoggerFactory()
            .AddSerilog();
        options.UseNpgsql(connectionStringBuilder.ConnectionString);
        options.UseLoggerFactory(loggerFactory);
        options.AddInterceptors(new SaveChangesInterceptor());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var applyGenericMethod =
            typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(c => c is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false })) 
        {
            foreach (var i in type.GetInterfaces())
            {
                if (!i.IsConstructedGenericType 
                    || i.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>)) continue;
                
                var applyConcreteMethod = applyGenericMethod?.MakeGenericMethod(i.GenericTypeArguments[0]);
                applyConcreteMethod?.Invoke(modelBuilder, new []
                {
                    Activator.CreateInstance(type)
                });
                break;
            }
        }
    }
}