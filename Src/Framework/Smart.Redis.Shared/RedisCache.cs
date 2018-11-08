using System.Configuration;
using StackExchange.Redis;
using Smart.Core.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smart.Core.Caching
{
    public class RedisCache : DisposableObject, ICache
    {
        IDatabase database;
        private List<string> _Keys = new List<string>();

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

        public object Get(string key)
        {
            string cacheValue = database.StringGet(key);
            if (cacheValue == null) return null;
            return cacheValue;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void ICache.Set(string key, CacheInfo cache)
        {
            string value = database.StringGet(key);
            if (value != null)
            {
                database.KeyDelete(key);
            }
            database.StringSet(key, cache.Value.ToJson(), flags: CommandFlags.None);
            if (!_Keys.Contains(key)) _Keys.Add(key);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(string key)
        {
            database.KeyDelete(key, CommandFlags.HighPriority);
        }

        public IEnumerable<string> GetAllKeys()
        {
            foreach (var item in _Keys)
            {
                yield return item;
            }
        }
     
    }
}
