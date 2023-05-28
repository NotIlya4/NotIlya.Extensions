using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace NotIlya.Extensions.SerilogExtensions;

public static class SerilogExtensions
{
    public static NAddSerilogOptions GetNAddSerilogOptions(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        string serviceName = config.GetRequiredValue("ServiceName");
        string? seqUrl = config.GetValue<string>("SeqUrl");

        return new NAddSerilogOptions(config, serviceName, seqUrl);
    }
    
    public static void NAddSerilog(this IServiceCollection services, NAddSerilogOptions options, Action<IServiceProvider, LoggerConfiguration>? configureSerilog = null)
    {
        services.AddHttpContextAccessor();
        services.AddSerilog((serilogServices, loggerConfigurator) =>
        {
            loggerConfigurator.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
            loggerConfigurator.Enrich.FromLogContext();
            loggerConfigurator.Enrich.WithProperty("ServiceName", options.ServiceName);
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