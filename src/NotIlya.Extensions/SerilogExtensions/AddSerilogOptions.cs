using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.Serilog;

public record AddSerilogOptions
{
    public required IConfiguration SerilogConfig { get; set; }
    public string? ServiceName { get; set; }
    public string? SeqUrl { get; set; }
}