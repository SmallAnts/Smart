using Smart.Core.Extensions;
using Smart.Core.Utilites;
using Smart.Core.WCF;
using System;
using System.Configuration;

namespace Smart.Core.Caching
{
    public class WcfCache : DisposableObject, ICache, IDisposable
    {
        private WcfClient<ICacheService> cacheClient;

        private ICacheService cacheService;

        public WcfCache()
        {
            string conn = ConfigurationManager.AppSettings["Smart:WcfCache"] ?? string.Format("{0}:9379", IPAddressUtility.GetLocalIPAddress());
            this.cacheClient = new WcfClient<ICacheService>(string.Format("net.tcp://{0}", conn), null, null);
            this.cacheService = this.cacheClient.CreateService();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.cacheService as IDisposable)?.Dispose();
                cacheClient.Dispose();
            }
        }

        public T Get<T>(string key) where T : class
        {
            string cacheValue = this.cacheService.Get(key);
            return cacheValue == null ? default(T) : cacheValue.JsonTo<T>();
        }

        public void Remove(string key)
        {
            this.cacheService.Remove(key);
        }

        public void RemoveAll(Predicate<string> match)
        {
            this.cacheService.RemoveAll(match);
        }

        public T Set<T>(string key, CacheInfo<T> cache) where T : class
        {
            this.cacheService.Set(key, cache.Value.ToJson(), cache.SlidingExpiration);
            return cache.Value;
        }

    }
}
