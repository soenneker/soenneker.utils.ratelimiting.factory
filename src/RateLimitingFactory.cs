using Soenneker.Utils.RateLimiting.Factory.Abstract;
using Soenneker.Utils.SingletonDictionary;
using Soenneker.Utils.RateLimiting.Executor;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Soenneker.Utils.RateLimiting.Factory;

/// <inheritdoc cref="IRateLimitingFactory"/>
public class RateLimitingFactory : IRateLimitingFactory
{
    private readonly SingletonDictionary<RateLimitingExecutor> _executors;

    public RateLimitingFactory()
    {
        _executors = new SingletonDictionary<RateLimitingExecutor>(args =>
        {
            var timeSpan = (TimeSpan) args[0];

            var executor = new RateLimitingExecutor(timeSpan);

            return executor;
        });
    }

    public ValueTask<RateLimitingExecutor> Get(string id, TimeSpan interval, CancellationToken cancellationToken = default)
    {
        return _executors.Get(id, cancellationToken, interval);
    }

    public RateLimitingExecutor GetSync(string id, TimeSpan interval, CancellationToken cancellationToken = default)
    {
        return _executors.GetSync(id, cancellationToken, interval);
    }

    public ValueTask Remove(string id)
    {
        return _executors.Remove(id);
    }

    public void RemoveSync(string id)
    {
        _executors.RemoveSync(id);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _executors.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _executors.Dispose();
    }
}