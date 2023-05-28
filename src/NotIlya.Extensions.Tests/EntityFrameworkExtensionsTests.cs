using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotIlya.Extensions.ConnectionStringExtensions;
using NotIlya.Extensions.EntityFrameworkExtensions;

namespace NotIlya.Extensions.Tests;

public class EntityFrameworkExtensionsTests
{
    private readonly ConfigurationManager _config = new ConfigurationManager();
    private readonly SqlConnectionStringBuilder _connectionString = new("Server=localhost,1433;Database=Products;User Id=SA;Multiple Active Result Sets=True;Trust Server Certificate=True;");

    public EntityFrameworkExtensionsTests()
    {
        _config["SqlServer:Database"] = "Products";
        _config["SqlServer:MigrationsAssembly"] = "Core";
        _config["SqlServer:QueryTrackingBehavior"] = QueryTrackingBehavior.TrackAll.ToString();
        _config["AutoMigrate"] = "True";
    }

    [Fact]
    public void GetNAddEfSqlServerOptions_ConnectionStringWithOtherProperties_ParseConnectionStringAndIgnorePropertiesNotRelatedToConnString()
    {
        NAddEfSqlServerOptions result = _config.GetNAddEfSqlServerOptions("SqlServer");
        
        Assert.Equal(_connectionString, new SqlConnectionStringBuilder(result.ConnectionString));
    }

    [Fact]
    public void GetNAddEfSqlServerOptions_SpecifiedMigrationsAssembly_ParseMigrationsAssembly()
    {
        NAddEfSqlServerOptions result = _config.GetNAddEfSqlServerOptions("SqlServer");
        
        Assert.Equal("Core", result.MigrationsAssembly);
    }
    
    [Fact]
    public void GetNAddEfSqlServerOptions_MigrationsAssemblyNotSpecified_MigrationsAssemblyNull()
    {
        ((IConfigurationBuilder)_config).Properties.Remove("MigrationsAssembly");
        
        NAddEfSqlServerOptions result = _config.GetNAddEfSqlServerOptions("SqlServer");
        
        Assert.Null(result.MigrationsAssembly);
    }

    [Fact]
    public void GetNAddEfSqlServerOptions_QueryTrackingBehaviorSpecified_ParseQueryTrackingBehavior()
    {
        NAddEfSqlServerOptions result = _config.GetNAddEfSqlServerOptions("SqlServer");
        
        Assert.Equal(QueryTrackingBehavior.TrackAll, result.QueryTrackingBehavior);
    }
    
    [Fact]
    public void GetNAddEfSqlServerOptions_QueryTrackingBehaviorNotSpecified_DefaultQueryTrackingBehaviorValue()
    {
        ((IConfigurationBuilder)_config).Properties.Remove("QueryTrackingBehavior");
        
        NAddEfSqlServerOptions result = _config.GetNAddEfSqlServerOptions("SqlServer");
        
        Assert.Equal(QueryTrackingBehavior.NoTracking, result.QueryTrackingBehavior);
    }

    [Fact]
    public void AutoMigrate_AutoMigrateSpecified_ParseAutoMigrate()
    {
        bool result = _config.AutoMigrate();
        Assert.True(result);

        _config["AutoMigrate"] = "False";
        bool result2 = _config.AutoMigrate();
        Assert.False(result2);
    }

    [Fact]
    public void AutoMigrate_AutoMigrateNotSpecified_ThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => _config.AutoMigrate("SqlServer"));
    }
}