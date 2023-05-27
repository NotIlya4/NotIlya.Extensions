﻿using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions;

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

        InvalidOperationException MakeException()
        {
            return new InvalidOperationException($"There is no such parameter in configuration in this path {configSection.Path}");
        }
        
        if (configSection.Value is null)
        {
            throw MakeException();
        }

        return config.Get<T>() ?? throw MakeException();
    }

    public static string GetRequiredValue(this IConfiguration config, string? key = null)
    {
        return config.GetRequiredValue<string>(key);
    }

    internal static IConfiguration ApplyKey(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            return config.GetSection(key);
        }

        return config;
    }

    internal static void IfContains<T>(this IConfiguration config, string ifContainsKey, Action<T> lambdaWhenContains)
    {
        config.IfContains(new [] { ifContainsKey }, lambdaWhenContains);
    }
    
    internal static void IfContains<T>(this IConfiguration config, IEnumerable<string> ifContainsKeys, Action<T> lambdaWhenContains)
    {
        foreach (string ifContainsKey in ifContainsKeys)
        {
            IConfigurationSection innerConfig = config.GetSection(ifContainsKey);

            if (innerConfig.Value is not null)
            {
                T? value = innerConfig.Get<T>();

                if (value is not null)
                {
                    lambdaWhenContains(value);
                }
            }
        }
    }

    internal static Dictionary<string, string> ToDict(this IConfiguration config)
    {
        return new Dictionary<string, string>(config.GetChildren().Select(s => new KeyValuePair<string, string>(s.Key, s.Value!)));
    }
}