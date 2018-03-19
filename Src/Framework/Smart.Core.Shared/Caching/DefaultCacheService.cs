using System;

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
            return this.cacheManager.Get<string>(key);
        }

        public void Remove(string key)
        {
            this.cacheManager.Remove(key);
        }

        public void RemoveAll(Predicate<string> match)
        {
            this.cacheManager.RemoveAll(match);
        }

        public void Set(string key, string cache, TimeSpan slidingExpiration)
        {
            this.cacheManager.Set<string>(key, new CacheInfo<string>(slidingExpiration, cache));
        }
    }
}
