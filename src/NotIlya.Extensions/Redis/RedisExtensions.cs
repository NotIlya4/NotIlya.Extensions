using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace NotIlya.Extensions.Redis;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(_ => ConnectionMultiplexer.Connect(connectionString));
        services.AddScoped(s => s.GetRequiredService<ConnectionMultiplexer>().GetDatabase());
        
        services.AddSingleton<IRedisClientFactory, RedisClientFactory>();
        services.AddSingleton<ISerializer, NewtonsoftSerializer>();

        services.AddSingleton((provider) => provider
            .GetRequiredService<IRedisClientFactory>()
            .GetDefaultRedisClient());

        services.AddSingleton((provider) => provider
            .GetRequiredService<IRedisClientFactory>()
            .GetDefaultRedisClient()
            .GetDefaultDatabase());

        services.AddSingleton(new RedisConfiguration() { ConnectionString = connectionString});
        
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, RedisConnectionStringBuilder connectionStringBuilder)
    {
        return services.AddRedis(connectionStringBuilder.ConnectionString);
    }
}