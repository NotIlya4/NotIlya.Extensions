using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotIlya.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;

namespace NotIlya.Extensions.Serilog;

public static class SerilogExtensions
{
    public static AddSerilogOptions GetAddSerilogOptions(this IConfiguration config, string key)
    {
        var innerConfig = config.GetSection(key);

        string? serviceName = innerConfig.GetValue<string>("ServiceName");
        string? seqUrl = innerConfig.GetValue<string>("SeqUrl");

        return new AddSerilogOptions()
        {
            SerilogConfig = config,
            SerilogConfigSectionName = key,
            ServiceName = serviceName,
            SeqUrl = seqUrl
        };
    }
    
    public static void AddSerilog(this IServiceCollection services, AddSerilogOptions options, Action<IServiceProvider, LoggerConfiguration>? configureSerilog = null)
    {
        services.AddHttpContextAccessor();
        services.AddSerilog((serilogServices, loggerConfigurator) =>
        {
            loggerConfigurator.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
            loggerConfigurator.Enrich.FromLogContext();
            if (options.ServiceName is not null)
            {
                loggerConfigurator.Enrich.WithProperty("ServiceName", options.ServiceName);
            }
            loggerConfigurator.Enrich.With<XRequestIdEnricher>();
            loggerConfigurator.WriteTo.Console();
            if (options.SeqUrl is not null)
            {
                loggerConfigurator.WriteTo.Seq(options.SeqUrl);
            }

            if (configureSerilog is not null)
            {
                configureSerilog(serilogServices, loggerConfigurator);
            }

            if (options.SerilogConfig is not null)
            {
                loggerConfigurator.ReadFrom.Configuration(options.SerilogConfig,
                    new ConfigurationReaderOptions()
                        { SectionName = options.SerilogConfigSectionName });
            }
        });
    }
}