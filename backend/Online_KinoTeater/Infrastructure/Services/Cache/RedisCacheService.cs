using Application.Interfaces.CacheService;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services.Cache;

public class RedisCacheService(IConnectionMultiplexer connection) : ICacheService
{
    private readonly IDatabase _dataBase = connection.GetDatabase();

    public async Task<T?> GetByKeyAsync<T>(string key)
    {
        var result = await _dataBase.StringGetAsync(key);
        return !result.IsNullOrEmpty ? JsonSerializer.Deserialize<T>(result!): default;
    }

    public async Task<long> IncrementAsync(string value) =>
        await _dataBase.StringIncrementAsync(value);

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan ttl) =>
        await _dataBase.StringSetAsync(
            key,
            JsonSerializer.Serialize(value),
            ttl);

    public async Task SetExpireAsync(string value, TimeSpan ttl) =>
        await _dataBase.KeyExpireAsync(value, ttl);

    public async Task<bool> KeyExistsAsync(string key) => 
        await _dataBase.KeyExistsAsync(key);

    public async Task<bool> DeleteByKeyAsync(string key) =>
        await _dataBase.KeyDeleteAsync(key);

    public async Task DeleteByPatternAsync(string pattern, CancellationToken cancellationToken)
    {
        var endpoint = connection
            .GetEndPoints()
            .First();

        var server = connection.GetServer(endpoint);

        var keys = server.Keys(pattern: pattern);

        var db = connection.GetDatabase();

        foreach (var key in keys)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await db.KeyDeleteAsync(key);
        }
    }
}

