using System;
using System.Configuration;
using StackExchange.Redis;
using Smart.Core.Extensions;

namespace Smart.Core.Caching
{
    public class RedisCache : DisposableObject, ICache
    {
        IDatabase database;

        ConnectionMultiplexer connectionMultiplexer;
        public RedisCache()
        {
            var address = ConfigurationManager.AppSettings["Smart:RedisServer"] ?? "127.0.0.1:6379";
            connectionMultiplexer = ConnectionMultiplexer.Connect(address);
            database = connectionMultiplexer.GetDatabase();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            connectionMultiplexer?.Close();
        }

        public T Get<T>(string key) where T : class
        {
            string cacheValue = database.StringGet(key);
            if (cacheValue == null) return null;
            return cacheValue.JsonTo<T>();
        }

        public void Remove(string key)
        {
            database.KeyDelete(key, CommandFlags.HighPriority);
        }

        public void RemoveAll(Predicate<string> match)
        {
            throw new NotImplementedException();
        }

        public T Set<T>(string key, CacheInfo<T> cache) where T : class
        {
            string value = database.StringGet(key);
            if (value != null)
            {
                database.KeyDelete(key);
            }
            database.StringSet(key, cache.Value.ToJson(), flags: CommandFlags.None);
            return cache.Value;
        }
    }
}
