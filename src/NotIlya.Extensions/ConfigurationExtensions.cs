﻿using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration config, string? key = null)
    {
        IConfigurationSection? configSection;
        if (key is not null)
        {
            configSection = config.GetSection(key);
        }
        else
        {
            configSection = config as IConfigurationSection ?? throw new InvalidOperationException("Config must be configuration section");
        }
        
        if (configSection.Value is null)
        {
            throw new InvalidOperationException($"There is no such parameter in configuration in this path {configSection.Path}");
        }

        return configSection.Get<T>() ?? throw new InvalidOperationException($"There is no such parameter in configuration in this path {configSection.Path}");
    }

    public static string GetRequiredValue(this IConfiguration config, string? key = null)
    {
        return config.GetRequiredValue<string>(key);
    }
    
    public static bool AutoMigrate(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        return config.GetRequiredValue<bool>("AutoMigrate");
    }

    internal static IConfiguration ApplyKey(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            return config.GetSection(key);
        }

        return config;
    }

    internal static Dictionary<string, string> ToDict(this IConfiguration config)
    {
        return new Dictionary<string, string>(config.GetChildren().Select(s => new KeyValuePair<string, string>(s.Key, s.Value!)));
    }
}