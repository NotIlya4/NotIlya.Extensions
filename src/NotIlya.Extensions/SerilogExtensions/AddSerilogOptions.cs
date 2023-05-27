using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.SerilogExtensions;

public record AddSerilogOptions
{
    public IConfiguration SerilogConfig { get; }
    public string ServiceName { get; }
    public string? SeqUrl { get; }

    public AddSerilogOptions(IConfiguration serilogConfig, string serviceName, string? seqUrl = null)
    {
        SerilogConfig = serilogConfig;
        ServiceName = serviceName;
        SeqUrl = seqUrl;
    }
}