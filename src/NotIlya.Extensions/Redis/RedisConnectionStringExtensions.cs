using Microsoft.Extensions.Configuration;
using NotIlya.Extensions.Configuration;

namespace NotIlya.Extensions.Redis;

public static class RedisConnectionStringExtensions
{
    public static RedisConnectionStringBuilder GetRedisConnectionStringBuilder(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        var builder = new RedisConnectionStringBuilder();
        builder.Server = "localhost";

        foreach (KeyValuePair<string,string> parameter in config.ToDict())
        {
            if (parameter.Key == "Server")
            {
                builder.Server = parameter.Value;
                continue;
            }
            builder.AddCustomParameter(parameter.Key, parameter.Value);
        }

        return builder;
    }

    public static string GetRedisConnectionString(this IConfiguration config, string? key = null)
    {
        return config.GetRedisConnectionStringBuilder(key).ConnectionString;
    }
}