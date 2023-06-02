using Microsoft.AspNetCore.Http;
using NotIlya.Extensions.String;
using Serilog.Core;
using Serilog.Events;

namespace NotIlya.Extensions.Serilog;

public class XRequestIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        HttpContext? context = new HttpContextAccessor().HttpContext;

        if (context is not null)
        {
            IHeaderDictionary headers = context.Request.Headers;
            
            if (headers.Any(k => k.Key.EqualsIgnoreCase("x-request-id")))
            {
                string header = headers.First(k => k.Key.EqualsIgnoreCase("x-request-id")).Value.ToString();
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("X-Request-Id", header));
            }
        }
    }
}