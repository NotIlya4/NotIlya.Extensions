namespace NotIlya.Extensions.Tests.ScopeExtensionsTests;

public class ExampleService : IDisposable, IAsyncDisposable
{
    public bool DisposeCalled { get; private set; } = false;
    public bool DisposeAsyncCalled { get; private set; } = false;
    
    public void Dispose()
    {
        DisposeCalled = true;
    }

    public async ValueTask DisposeAsync()
    {
        DisposeAsyncCalled = true;
    }
}