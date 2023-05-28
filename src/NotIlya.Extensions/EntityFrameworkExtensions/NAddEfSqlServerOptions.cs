using Microsoft.EntityFrameworkCore;

namespace NotIlya.Extensions.EntityFrameworkExtensions;

public class NAddEfSqlServerOptions
{
    private const QueryTrackingBehavior DefaultTrackingBehavior = QueryTrackingBehavior.NoTracking;
    public string ConnectionString { get; set; }
    public QueryTrackingBehavior QueryTrackingBehavior { get; set; }
    public string? MigrationsAssembly { get; set; }

    public NAddEfSqlServerOptions(string connectionString, QueryTrackingBehavior queryTrackingBehavior = DefaultTrackingBehavior, string? migrationsAssembly = null)
    {
        ConnectionString = connectionString;
        QueryTrackingBehavior = queryTrackingBehavior;
        MigrationsAssembly = migrationsAssembly;
    }

    public static NAddEfSqlServerOptions Create(string connectionString, string? queryTrackingBehavior = null, string? migrationsAssembly = null)
    {
        if (queryTrackingBehavior is not null)
        {
            return new NAddEfSqlServerOptions(
                connectionString: connectionString,
                queryTrackingBehavior: Enum.Parse<QueryTrackingBehavior>(queryTrackingBehavior, true),
                migrationsAssembly: migrationsAssembly);
        }

        return new NAddEfSqlServerOptions(
            connectionString: connectionString,
            queryTrackingBehavior: DefaultTrackingBehavior,
            migrationsAssembly: migrationsAssembly);
    }
}