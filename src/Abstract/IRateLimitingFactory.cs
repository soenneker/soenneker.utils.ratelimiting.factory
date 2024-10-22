using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Threading;
using Soenneker.Utils.RateLimiting.Executor;

namespace Soenneker.Utils.RateLimiting.Factory.Abstract;

/// <summary>
/// An async thread-safe singleton dictionary for Soenneker.Utils.RateLimiting.Executors, designed to manage the rate at which tasks are executed.
/// </summary>
public interface IRateLimitingFactory : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Retrieves a rate-limiting executor asynchronously, creating one if it doesn't exist for the given ID.
    /// </summary>
    /// <param name="id">A unique identifier for the rate-limiting executor.</param>
    /// <param name="interval">The time interval for the rate limiter.</param>
    /// <param name="cancellationToken">A token to cancel the operation, if necessary.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> representing the rate-limiting executor.</returns>
    [Pure]
    ValueTask<RateLimitingExecutor> Get(string id, TimeSpan interval, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a rate-limiting executor synchronously, creating one if it doesn't exist for the given ID.
    /// </summary>
    /// <param name="id">A unique identifier for the rate-limiting executor.</param>
    /// <param name="interval">The time interval for the rate limiter.</param>
    /// <param name="cancellationToken">A token to cancel the operation, if necessary.</param>
    /// <returns>The rate-limiting executor associated with the given ID.</returns>
    [Pure]
    RateLimitingExecutor GetSync(string id, TimeSpan interval, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a rate-limiting executor asynchronously by its ID.
    /// </summary>
    /// <param name="id">A unique identifier for the rate-limiting executor to remove.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Remove(string id);

    /// <summary>
    /// Removes a rate-limiting executor synchronously by its ID.
    /// </summary>
    /// <param name="id">A unique identifier for the rate-limiting executor to remove.</param>
    void RemoveSync(string id);
}
