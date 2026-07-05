using JobBoard.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace JobBoard.Infrastructure.Implementation.RedisImplementation
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;
        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public T? GetData<T>(string key)
        {
            var value = _db.StringGet(key);

            if (!value.IsNullOrEmpty)
                return JsonSerializer.Deserialize<T>(value!);

            return default;
        }
        public bool SetData<T>(string key, T value, TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(value);
            return _db.StringSet(key, json, expiration);
        }
        public bool RemoveData(string key)
        {

            var exist = _db.KeyExists(key);
            if (exist)
                return _db.KeyDelete(key);

            return false;
        }
    }
}