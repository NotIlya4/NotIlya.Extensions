using Microsoft.Extensions.Configuration;
using NotIlya.Extensions.Redis;

namespace NotIlya.Extensions.Tests.Redis;

public class RedisConnectionStringExtensionsTests
{
    private readonly ConfigurationManager _config = new ConfigurationManager();
    
    public RedisConnectionStringExtensionsTests()
    {
        _config["Redis:Server"] = "localhost:141";
        _config["Redis:defaultDatabase"] = "4";
        _config["Redis:password"] = "IPassword";
        _config["Redis:allowAdmin"] = "false";
    }

    [Fact]
    public void GetRedisConnectionStringBuilder_ConfigurationWithConnectionString_ParseIt()
    {
        var expect = new RedisConnectionStringBuilder("localhost:141,defaultDatabase=4,password=IPassword,allowAdmin=false");

        RedisConnectionStringBuilder result = _config.GetRedisConnectionStringBuilder("Redis");

        Assert.Equal(expect, result);
    }

    [Fact]
    public void GetRedisConnectionStringBuilder_EmptyConfig_DefaultValues()
    {
        var config = new ConfigurationManager();

        RedisConnectionStringBuilder result = config.GetRedisConnectionStringBuilder();

        Assert.Equal("localhost", result.ConnectionString);
    }
}