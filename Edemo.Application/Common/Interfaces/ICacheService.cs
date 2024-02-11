namespace Edemo.Application.Common.Interfaces;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? expirationTime = null);
    T? GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expirationTime = null);
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expirationTime = null);
    void Remove(string key);
}