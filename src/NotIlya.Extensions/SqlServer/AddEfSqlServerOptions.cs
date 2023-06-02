using Microsoft.EntityFrameworkCore;

namespace NotIlya.Extensions.SqlServer;

public class AddEfSqlServerOptions
{
    public string ConnectionString { get; set; }
    public QueryTrackingBehavior QueryTrackingBehavior { get; set; } = QueryTrackingBehavior.TrackAll;
    public string? MigrationsAssembly { get; set; }

    public AddEfSqlServerOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }

    internal AddEfSqlServerOptions(string connectionString, string? queryTrackingBehavior)
    {
        if (queryTrackingBehavior is not null)
        {
            QueryTrackingBehavior = Enum.Parse<QueryTrackingBehavior>(queryTrackingBehavior, true);
        }

        ConnectionString = connectionString;
    }
}