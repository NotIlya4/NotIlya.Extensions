using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotIlya.Extensions.Configuration;

namespace NotIlya.Extensions.SqlServer;

public static class SqlServerDiExtensions
{
    public static AddEfSqlServerOptions GetAddEfSqlServerOptions(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        var connectionStringConfig = new ConfigurationManager();
        foreach (IConfigurationSection child in config.GetChildren())
        {
            if (!new[] { "MigrationsAssembly", "QueryTrackingBehavior" }.Contains(child.Key))
            {
                connectionStringConfig[child.Key] = child.Value;
            }
        }
        
        string connectionString = connectionStringConfig.GetSqlConnectionString();
        string? migrationsAssembly = config.GetValue<string>("MigrationsAssembly");
        string? queryTrackingBehavior = config.GetValue<string>("QueryTrackingBehavior");

        return new AddEfSqlServerOptions(connectionString, queryTrackingBehavior)
        {
            MigrationsAssembly = migrationsAssembly
        };
    }
    
    public static IServiceCollection AddEfSqlServer<TDbContext>(this IServiceCollection services, AddEfSqlServerOptions options) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(builder =>
        {
            builder.UseSqlServer(options.ConnectionString, optionsBuilder =>
            {
                if (options.MigrationsAssembly is not null)
                {
                    optionsBuilder.MigrationsAssembly(options.MigrationsAssembly);
                }
            });
            builder.UseQueryTrackingBehavior(options.QueryTrackingBehavior);
        });
        return services;
    }
}