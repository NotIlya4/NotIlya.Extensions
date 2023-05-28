using Microsoft.Extensions.DependencyInjection;

namespace NotIlya.Extensions.Tests;

public class ScopeExtensionsTests
{
    private readonly IServiceProvider _services;

    public ScopeExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddScoped<ExampleService>();
        _services = services.BuildServiceProvider();
    }

    [Fact]
    public void UsingScope_GetService_CallDisposeOnExit()
    {
        ExampleService service1 = new ExampleService();
        ExampleService service2 = new ExampleService();
        
        _services.UsingScope(services =>
        {
            service1 = services.GetRequiredService<ExampleService>();
            service2 = services.GetRequiredService<ExampleService>();
        });
        
        Assert.True(service1.DisposeCalled);
        Assert.True(service2.DisposeCalled);
    }

    [Fact]
    public void UsingScope_UseMethodWithGeneric_CallDisposeOnExit()
    {
        ExampleService service = new ExampleService();
        
        _services.UsingScope<ExampleService>(exampleService =>
        {
            service = exampleService;
        });
        
        Assert.True(service.DisposeCalled);
    }

    [Fact]
    public async Task UsingScopeAsync_GetService_CallDisposeAsyncOnExit()
    {
        ExampleService service1 = new ExampleService();
        ExampleService service2 = new ExampleService();
        
        await _services.UsingScopeAsync(async services =>
        {
            service1 = services.GetRequiredService<ExampleService>();
            service2 = services.GetRequiredService<ExampleService>();
        });
        
        Assert.True(service1.DisposeAsyncCalled);
        Assert.True(service2.DisposeAsyncCalled);
    }
    
    [Fact]
    public async Task UsingScopeAsync_UseMethodWithGeneric_CallDisposeAsyncOnExit()
    {
        ExampleService service = new ExampleService();
        
        await _services.UsingScopeAsync<ExampleService>(async exampleService =>
        {
            service = exampleService;
        });
        
        Assert.True(service.DisposeAsyncCalled);
    }
}