using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;

public class CartService : ICartService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public CartService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        RedisValue redisValue = await _database.StringGetAsync(key);
        return redisValue.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(redisValue!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart),expiry: TimeSpan.FromHours(24));
        if (!created) return null;

        return await GetCartAsync(cart.Id);
    }
}
