using Smart.Core.Extensions;
using Smart.Core.Utilites;
using Smart.Core.WCF;
using System;
using System.Collections.Generic;
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

        public object Get(string key)
        {
            string cacheValue = this.cacheService.Get(key);
            return cacheValue;
        }
        void ICache.Set(string key, CacheInfo cache)
        {
            this.cacheService.Set(key, cache.Value.ToJson(), cache.SlidingExpiration);
        }

        public void Remove(string key)
        {
            this.cacheService.Remove(key);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return this.cacheService.GetAllKeys();
        }

    }
}
