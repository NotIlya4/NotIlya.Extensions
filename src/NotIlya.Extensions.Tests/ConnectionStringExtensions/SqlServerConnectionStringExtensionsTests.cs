using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotIlya.Extensions.ConnectionStringExtensions;

namespace NotIlya.Extensions.Tests;

public class SqlServerConnectionStringExtensionsTests
{
    private readonly ConfigurationManager _config;

    public SqlServerConnectionStringExtensionsTests()
    {
        _config = new ConfigurationManager();

        _config["SqlServer:Server"] = "localhost,14333";
        _config["SqlServer:Database"] = "Biba";
        _config["SqlServer:User Id"] = "SSAA";
        _config["SqlServer:Password"] = "asd";
    }

    [Fact]
    public void GetSqlConnectionStringBuilder_ConfigurationManagerWithConnectionString_BuiltConnectionString()
    {
        _config["SqlServer:MultipleActiveResultSets"] = "false";
        _config["SqlServer:TrustServerCertificate"] = "false";
        var expect = new SqlConnectionStringBuilder("Server=localhost,14333;Database=Biba;User Id=SSAA;Password=asd");
        expect.MultipleActiveResultSets = false;
        expect.TrustServerCertificate = false;
        
        SqlConnectionStringBuilder result = _config.GetSqlConnectionStringBuilder("SqlServer");
        
        Assert.Equal(expect, result);
    }

    [Fact]
    public void GetSqlConnectionStringBuilder_EmptyConfig_PropagateWithDefaultValues()
    {
        var config = new ConfigurationManager();
        var expect = new SqlConnectionStringBuilder("Server=localhost,1433;User Id=SA");
        expect.MultipleActiveResultSets = true;
        expect.TrustServerCertificate = true;

        SqlConnectionStringBuilder result = config.GetSqlConnectionStringBuilder("SqlServer");
        
        Assert.Equal(expect, result);
    }
}