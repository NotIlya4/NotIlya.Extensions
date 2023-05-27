# NotIlya.Extensions
This is a pack of my extensions that I grew tired of copying from one project to another. Its not tied to any specific topic there are extensions for different cases. Currently readme isn't ready I will add description to each extension method later.

## Instalation
ExceptionCatcherMiddleware is [available on NuGet](https://www.nuget.org/packages/NotIlya.Extensions) and can be installed via the below commands:
```
$ Install-Package NotIlya.Extensions
```
or via the .NET Core CLI:

```
$ dotnet add package NotIlya.Extensions
```

## Extensions
```csharp
public static T GetRequiredValue<T>(this IConfiguration config, string? key = null)
```
```csharp
public static SqlConnectionStringBuilder GetSqlConnectionStringBuilder(this IConfiguration config, string? key = null)

public static string GetSqlConnectionString(this IConfiguration config, string? key = null)
```
```csharp
public static RedisConnectionStringBuilder GetRedisConnectionStringBuilder(this IConfiguration config, string? key = null)

public static string GetRedisConnectionString(this IConfiguration config, string? key = null)
```
```csharp
public static AddSerilogOptions GetAddConfigurationOptions(this IConfiguration config, string? key = null)

public static void AddConfiguredSerilog(this IServiceCollection services, AddSerilogOptions options, Action<IServiceProvider, LoggerConfiguration>? configureSerilog = null)
```
```csharp
public static void AddConfiguredEfSqlServer<TDbContext>(this IServiceCollection services, string connectionString, QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.NoTracking)
```