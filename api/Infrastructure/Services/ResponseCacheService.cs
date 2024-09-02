using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;

public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
{
    private readonly IDatabase _database = redis.GetDatabase(1);
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var serialiazedResponse = JsonSerializer.Serialize(response, options);

        await _database.StringSetAsync(cacheKey, serialiazedResponse, timeToLive);
    }

    public async Task<string?> GetCachedResponseAsync(string key)
    {
        var cachedResponse = await _database.StringGetAsync(key);

        if (cachedResponse.IsNullOrEmpty) return null;

        return cachedResponse;
    }

    public async Task RemoveCacheByPattern(string pattern)
    {
        var server = redis.GetServer(redis.GetEndPoints().First());
        var keys = server.Keys(database : 1, pattern : $"*{pattern}*").ToArray();

        if (keys.Length != 0)
            await _database.KeyDeleteAsync(keys);
    }
}
