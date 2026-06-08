namespace Application.Interfaces.CacheService;

public interface ICacheService
{
    Task<bool> SetAsync<T>(string key, T value, TimeSpan tll);

    Task<T?> GetByKeyAsync<T>(string value);

    Task<bool> KeyExistsAsync(string key);

    public Task SetExpireAsync(string value, TimeSpan ttl);

    Task<long> IncrementAsync(string value);

    Task<bool> DeleteByKeyAsync(string key);

    Task DeleteByPatternAsync(string pattern, CancellationToken cancellationToken);

}
