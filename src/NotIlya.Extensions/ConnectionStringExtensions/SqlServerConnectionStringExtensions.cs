using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotIlya.Extensions.ConnectionStringExtensions;

public static class SqlServerConnectionStringExtensions
{
    public static SqlConnectionStringBuilder GetSqlConnectionStringBuilder(this IConfiguration config, string? key = null)
    {
        config = config.ApplyKey(key);

        var builder = new SqlConnectionStringBuilder();
        builder.DataSource = "localhost,1433";
        builder.UserID = "SA";
        builder.MultipleActiveResultSets = true;
        builder.TrustServerCertificate = true;

        foreach (KeyValuePair<string,string> parameter in config.ToDict())
        {
            builder.Add(parameter.Key, parameter.Value);
        }

        return builder;
    }

    public static string GetSqlConnectionString(this IConfiguration config, string? key = null)
    {
        return config.GetSqlConnectionStringBuilder(key).ConnectionString;
    }
}