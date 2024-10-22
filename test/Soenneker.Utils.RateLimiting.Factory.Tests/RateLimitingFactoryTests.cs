using Soenneker.Utils.RateLimiting.Factory.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using Soenneker.Utils.RateLimiting.Executor;
using System;
using Microsoft.Extensions.Logging;

namespace Soenneker.Utils.RateLimiting.Factory.Tests;

[Collection("Collection")]
public class RateLimitingFactoryTests : FixturedUnitTest
{
    private readonly IRateLimitingFactory _factory;

    public RateLimitingFactoryTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _factory = Resolve<IRateLimitingFactory>(true);
    }

    [Fact]
    public async Task Execute_should_execute_in_order()
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
