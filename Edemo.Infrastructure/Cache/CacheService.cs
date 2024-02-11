using Edemo.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Edemo.Infrastructure.Cache;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public T? Get<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        if (expirationTime.HasValue)
        {
            cacheEntryOptions.SetAbsoluteExpiration(expirationTime.Value);
        }
        memoryCache.Set(key, value, cacheEntryOptions);
    }

    public T? GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expirationTime = null)
    {
        var gotValue =  memoryCache.TryGetValue(key, out T? value);
        if (gotValue == false)
        {
            value = factory();
            Set(key,value,expirationTime);
        }

        return value;
    }

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expirationTime = null)
    {
        var gotValue =  memoryCache.TryGetValue(key, out T? value);
        if (gotValue == false)
        {
            value = await factory();
            Set(key,value,expirationTime);
        }

        return value;
    }

    public void Remove(string key)
    {
        memoryCache.Remove(key);
    }
}