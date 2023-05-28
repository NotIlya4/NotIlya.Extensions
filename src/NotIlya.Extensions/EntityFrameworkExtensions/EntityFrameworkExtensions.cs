using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotIlya.Extensions.ConnectionStringExtensions;

namespace NotIlya.Extensions.EntityFrameworkExtensions;

public static class EntityFrameworkExtensions
{
    public static NAddEfSqlServerOptions GetNAddEfSqlServerOptions(this IConfiguration config, string? key = null)
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
        
        return NAddEfSqlServerOptions.Create(
            connectionString: connectionString,
            queryTrackingBehavior: queryTrackingBehavior,
            migrationsAssembly: migrationsAssembly);
    }
    
    public static void NAddEfSqlServer<TDbContext>(this IServiceCollection services, NAddEfSqlServerOptions options) where TDbContext : DbContext
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
    }
}