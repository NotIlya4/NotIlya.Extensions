using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.Serilog;

public class AddSerilogOptions
{
    public IConfiguration? SerilogConfig { get; set; }
    public string? SerilogConfigSectionName { get; set; } = "Serilog";
    public string? ServiceName { get; set; }
    public string? SeqUrl { get; set; }
}