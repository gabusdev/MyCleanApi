using Application.Common.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Caching;

public class LocalCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public LocalCacheService(IMemoryCache cache, ILogger<LocalCacheService> logger) =>
        _cache = cache;

    public T? Get<T>(string key) =>
        _cache.Get<T>(key);

    public Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
        Task.FromResult(Get<T>(key));

    public void Refresh(string key) =>
        _cache.TryGetValue(key, out _);

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);
        return Task.CompletedTask;
    }

    public void Remove(string key) =>
        _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null)
    {
        _cache.Set(key, value, GetOptions(slidingExpiration, absoluteExpiration));
        Log.Debug($"Added to Cache : {key}", key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken token = default)
    {
        Set(key, value, slidingExpiration, absoluteExpiration);
        return Task.CompletedTask;
    }

    private static MemoryCacheEntryOptions GetOptions(TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration)
    {
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpiration.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpiration.Value);
        }
        else
        {
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                // TODO: add to appsettings?
                options.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Default expiration time of 10 minutes.
            }
        }

        return options;
    }
}