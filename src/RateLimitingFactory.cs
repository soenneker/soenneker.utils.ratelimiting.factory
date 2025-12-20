using Soenneker.Utils.RateLimiting.Factory.Abstract;
using Soenneker.Utils.SingletonDictionary;
using Soenneker.Utils.RateLimiting.Executor;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Soenneker.Utils.RateLimiting.Factory;

/// <inheritdoc cref="IRateLimitingFactory"/>
public sealed class RateLimitingFactory : IRateLimitingFactory
{
    private readonly SingletonDictionary<RateLimitingExecutor, TimeSpan> _executors;

    public RateLimitingFactory()
    {
        _executors = new SingletonDictionary<RateLimitingExecutor, TimeSpan>(timeSpan =>
        {
            var executor = new RateLimitingExecutor(timeSpan);

            return executor;
        });
    }

    public ValueTask<RateLimitingExecutor> Get(string id, TimeSpan interval, CancellationToken cancellationToken = default)
    {
        return _executors.Get(id, interval, cancellationToken);
    }

    public RateLimitingExecutor GetSync(string id, TimeSpan interval, CancellationToken cancellationToken = default)
    {
        return _executors.GetSync(id, interval, cancellationToken);
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
        return _executors.DisposeAsync();
    }

    public void Dispose()
    {
        _executors.Dispose();
    }
}