# NotIlya.Extensions
⚠️ **Deprecated**. I decided to separate extensions to dedicated nuget packages.

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

## Serilog Extensions
There is one extension method for `IServiceCollection`:
```csharp
void AddSerilog(this IServiceCollection services, AddSerilogOptions options,
    Action<IServiceProvider, LoggerConfiguration>? configureSerilog = null)
```
It has `AddSerilogOptions` class, you can create instance yourself or by using `IConfiguration` extension:
```csharp
AddSerilogOptions GetAddSerilogOptions(this IConfiguration config, string key)
```
Assume we are going to do:
```csharp
configuration.GetAddSerilogOptions("SerilogOptions");
```
Then `IConfiguration` must has following structure:
```json
{
  "SerilogOptions": {
    "ServiceName": "Gateway",
    "SeqUrl": "http://localhost:5341",
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    ... and other serilog options
  }
}
```
You can add configured serilog with some predefined enrichers, sinks and other staff. Currently there are:
- `x-request-id Enricher` It searches for x-request-id header in request and use it while logging.
- `ServiceName Enricher` You can provide service name to `` in options and it will be added in log properties.
- `Seq Logging` You can specify seq url and it will log to seq.

## Redis Extension
There is one `IServiceCollection` extension that you can use to add Redis clients to your app:
```csharp
IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
```
It adds clients from `StackExchange.Redis`, `StackExchange.Redis.Extensions.Core` and `StackExchange.Redis.Extensions.Newtonsoft`.
And now in services you can use `IDatabase` and `IRedisDatabase`.

You can provide connection string either yourself or by using `IConfiguration` extension:
```csharp
string GetRedisConnectionString(this IConfiguration config, string? key = null)
```
This is a dumb implementation of that extension since it doesn't validates any inputs, it just reads all your properties and concats then with comma.
For example if call it like this:
```csharp
configuration.GetRedisConnectionString("Redis");
```
You will need to has config like this:
```json
{
  "Redis": {
    "Server": "localhost:6380,redis1:6380",
    "allowAdmin": true,
    "user": "admin",
    "password": "asdasdasd"
  }
}
```
It will be serialized to this: `localhost:6380,redis1:6380,allowAdmin=true,user=admin,password=asdasdasd`. As you can see Server property omits its name but its the only exception.
## Entity Framework Sql Server Extensions
You can add entity framework with sql server using:
```csharp
IServiceCollection AddEfSqlServer<TDbContext>(this IServiceCollection services, 
    AddEfSqlServerOptions options) where TDbContext : DbContext
```
You can either create options instance yourself or using `IConfiguration` extension:
```csharp
AddEfSqlServerOptions GetAddEfSqlServerOptions(this IConfiguration config, string? key = null)
```
If you run:
```csharp
configuration.GetAddEfSqlServerOptions("SqlServer");
```
You will need to have config like this:
```json
{
  "SqlServer": {
    "MigrationsAssembly": "Infrastructure",
    "QueryTrackingBehavior": "TrackAll",
    "Server": "localhost:1433",
    "Database": "Test",
    "User Id": "user",
    "Password": "asdasdasd",
    ... and other sql server connection string properties
  }
}
```
