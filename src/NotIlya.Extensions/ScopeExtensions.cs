using Microsoft.Extensions.DependencyInjection;

namespace NotIlya.Extensions.ServiceProvider;

public static class ScopeExtensions
{
    public static void UsingScope(this IServiceProvider services, Action<IServiceProvider> scopeAction)
    {
        IServiceScope scope = services.CreateScope();
        scopeAction(scope.ServiceProvider);
        scope.Dispose();
    }
    
    public static void UsingScope<TService>(this IServiceProvider services, Action<TService> scopeAction) where TService : notnull
    {
        IServiceScope scope = services.CreateScope();
        scopeAction(scope.ServiceProvider.GetRequiredService<TService>());
        scope.Dispose();
    }

    public static async Task UsingScopeAsync(this IServiceProvider services, Func<IServiceProvider, Task> asyncScopeAction)
    {
        AsyncServiceScope asyncScope = services.CreateAsyncScope();
        await asyncScopeAction(asyncScope.ServiceProvider);
        await asyncScope.DisposeAsync();
    }

    public static async Task UsingScopeAsync<TService>(this IServiceProvider services,
        Func<TService, Task> asyncScopeAction) where TService : notnull
    {
        AsyncServiceScope asyncScope = services.CreateAsyncScope();
        await asyncScopeAction(asyncScope.ServiceProvider.GetRequiredService<TService>());
        await asyncScope.DisposeAsync();
    }
}