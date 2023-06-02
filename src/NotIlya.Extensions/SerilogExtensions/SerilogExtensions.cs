using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotIlya.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace NotIlya.Extensions.Serilog;

public static class SerilogExtensions
{
    public static AddSerilogOptions GetAddSerilogOptions(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        string? serviceName = config.GetValue<string>("ServiceName");
        string? seqUrl = config.GetValue<string>("SeqUrl");

        return new AddSerilogOptions()
        {
            SerilogConfig = config,
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
            
            loggerConfigurator.ReadFrom.Configuration(options.SerilogConfig);
        });
    }
}