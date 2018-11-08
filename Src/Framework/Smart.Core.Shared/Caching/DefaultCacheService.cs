using System;
using System.Collections.Generic;
using Smart.Core.Extensions;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 默认的 WCF 缓存服务实现 
    /// </summary>
    public class DefaultCacheService : ICacheService
    {
        private ICache cacheManager;

        public DefaultCacheService()
        {
            this.cacheManager = new MemoryCache();
        }

        public string Get(string key)
        {
            return this.cacheManager.Get(key).AsString();
        }

        public void Set(string key, string value, TimeSpan slidingExpiration)
        {
            this.cacheManager.Set(key, new CacheInfo(key, value, slidingExpiration));
        }

        public void Remove(string key)
        {
            this.cacheManager.Remove(key);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return this.cacheManager.GetAllKeys();
        }

    }
}
