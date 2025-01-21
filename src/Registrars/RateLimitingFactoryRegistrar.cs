using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.RateLimiting.Factory.Abstract;

namespace Soenneker.Utils.RateLimiting.Factory.Registrars;

/// <summary>
/// An async thread-safe singleton dictionary for Soenneker.Utils.RateLimiting.Executors, designed to manage the rate at which tasks are executed.
/// </summary>
public static class RateLimitingFactoryRegistrar
{
    /// <summary>
    /// Adds <see cref="IRateLimitingFactory"/> as a singleton service. <para/>
    /// </summary>
    /// <remarks>This is most likely what you want.</remarks>
    public static IServiceCollection AddRateLimitingFactoryAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IRateLimitingFactory, RateLimitingFactory>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IRateLimitingFactory"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddRateLimitingFactoryAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IRateLimitingFactory, RateLimitingFactory>();

        return services;
    }
}