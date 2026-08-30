[![](https://img.shields.io/nuget/v/soenneker.utils.ratelimiting.factory.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.ratelimiting.factory/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.ratelimiting.factory/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.ratelimiting.factory/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.utils.ratelimiting.factory.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.ratelimiting.factory/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.ratelimiting.factory/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.ratelimiting.factory/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.RateLimiting.Factory

Provides one shared `RateLimitingExecutor` per string ID.

## Installation

```bash
dotnet add package Soenneker.Utils.RateLimiting.Factory
```

## Registration

```csharp
using Soenneker.Utils.RateLimiting.Factory.Registrars;

services.AddRateLimitingFactoryAsSingleton();
```

Singleton registration is the normal choice when an ID represents an application-wide upstream
limit. Scoped registration creates a separate dictionary and separate executors in every scope, so
requests in different scopes do not throttle one another.

## Get and use a keyed executor

```csharp
RateLimitingExecutor executor = await factory.Get(
    id: "vendor-api",
    interval: TimeSpan.FromSeconds(2),
    cancellationToken);

ApiResponse response = await executor.Execute(
    token => apiClient.Send(token),
    cancellationToken);
```

The ID is the cache key. Concurrent calls for the same ID receive the same executor; different IDs
have independent locks and schedules. The interval supplied when an ID is first created wins.
Later calls for that ID return the existing executor and do not reconfigure it, so use stable IDs
and pass a consistent interval.

The factory owns cached executors. Callers should use them but must not dispose or permanently
cancel them. The cancellation token passed to `Get` applies while obtaining the cached value; pass
the operation token again to `Execute` for waiting and delegate cancellation. `GetSync` provides
the same cache semantics for synchronous acquisition.

## Remove an executor

```csharp
bool removed = await factory.Remove("vendor-api");
```

Removal coordinates with in-flight creation, removes the keyed executor, and disposes it. Any
caller still holding that instance now has a canceled/disposed executor, so remove only after its
users have been coordinated. A later `Get` for the same ID creates a new executor and establishes a
new interval. `Remove` returns `false` when no value was present; `RemoveSync` is the synchronous
equivalent without a result.

Disposing the factory removes and disposes all cached executors. Let dependency injection own that
lifetime. Because IDs remain cached until removal or factory disposal, avoid unbounded IDs derived
from individual requests or users.

For execution spacing, delegate forms, and terminal cancellation behavior, see
[Soenneker.Utils.RateLimiting.Executor](https://github.com/soenneker/soenneker.utils.ratelimiting.executor).
