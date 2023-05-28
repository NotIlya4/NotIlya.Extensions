using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.SerilogExtensions;

public record NAddSerilogOptions
{
    public IConfiguration SerilogConfig { get; }
    public string ServiceName { get; }
    public string? SeqUrl { get; }

    public NAddSerilogOptions(IConfiguration serilogConfig, string serviceName, string? seqUrl = null)
    {
        SerilogConfig = serilogConfig;
        ServiceName = serviceName;
        SeqUrl = seqUrl;
    }
}