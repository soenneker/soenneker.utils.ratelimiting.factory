[![](https://img.shields.io/nuget/v/soenneker.utils.ratelimiting.factory.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.ratelimiting.factory/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.ratelimiting.factory/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.ratelimiting.factory/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.utils.ratelimiting.factory.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.ratelimiting.factory/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.RateLimiting.Factory
### An async thread-safe singleton dictionary for [Soenneker.Utils.RateLimiting.Executors](`https://github.com/soenneker/soenneker.utils.ratelimiting.executor), designed to manage the rate at which tasks are executed.

## Installation

```
dotnet add package Soenneker.Utils.RateLimiting.Factory
```

## Usage

1. Register `IRateLimitingFactory` within DI (`Program.cs`).

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddRateLimitingFactoryAsSingleton();
}
```

2. Inject `IRateLimitingFactory`, and retrieve a `RateLimitingFactory`.

Example:

```csharp
public class TestClass
{
    IRateLimitingFactory _factory;

    public TestClass(IRateLimitingFactory factory)
    {
        _factory = factory;
    }

    public async ValueTask ExecuteTasks()
    {
        RateLimitingExecutor rateLimitingExecutor = await _factory.Get("test", TimeSpan.FromSeconds(2));

        for (int i = 0; i < 5; i++)
        {
            await rateLimitingExecutor.Execute(async ct =>
            {
                Logger.LogInformation($"Executing Task {i + 1} at {DateTime.Now:HH:mm:ss}");

                await Task.Delay(100, ct); // Simulate some work
            });
        }
    }
}
```

### Console Output

```
Executing Task 1 at 14:00:00
Executing Task 2 at 14:00:02
Executing Task 3 at 14:00:04
Executing Task 4 at 14:00:06
Executing Task 5 at 14:00:08
```