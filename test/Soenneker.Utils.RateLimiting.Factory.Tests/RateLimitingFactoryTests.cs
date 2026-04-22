using Soenneker.Utils.RateLimiting.Factory.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;
using Soenneker.Utils.RateLimiting.Executor;
using System;
using Microsoft.Extensions.Logging;

namespace Soenneker.Utils.RateLimiting.Factory.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class RateLimitingFactoryTests : HostedUnitTest
{
    private readonly IRateLimitingFactory _factory;

    public RateLimitingFactoryTests(Host host) : base(host)
    {
        _factory = Resolve<IRateLimitingFactory>(true);
    }

    [Test]
    public void Default()
    {
    }

    [Test]
    public async Task Execute_should_execute_in_order()
    {
        RateLimitingExecutor rateLimitingExecutor = await _factory.Get("test", TimeSpan.FromSeconds(2), CancellationToken);

        for (int i = 0; i < 5; i++)
        {
            await rateLimitingExecutor.Execute(async ct =>
            {
                Logger.LogInformation($"Executing Task {i + 1} at {DateTime.Now:HH:mm:ss}");

                await Task.Delay(100, ct); // Simulate some work
            }, CancellationToken);
        }
    }
}
