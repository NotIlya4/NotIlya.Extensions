using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NotIlya.Extensions.ServiceExtensions;

public static class EntityFrameworkExtensions
{
    public static void AddConfiguredEfSqlServer<TDbContext>(this IServiceCollection services, string connectionString, QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.NoTracking) 
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString);
            builder.UseQueryTrackingBehavior(queryTrackingBehavior);
        });
    }
}