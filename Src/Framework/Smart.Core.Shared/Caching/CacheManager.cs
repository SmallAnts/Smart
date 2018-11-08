using Smart.Core.Extensions;
using System;
using System.Collections.Concurrent;

namespace Smart.Core.Caching
{
    public class CacheManager
    {
        private CacheManager() { }

        public static CacheManager Current { get; private set; } = new CacheManager();

        private static Lazy<ICache> _MemoryCacheService
            = new Lazy<ICache>(() => SmartContext.Current.Resolve<ICache>("MemoryCache"), true);

        private static Lazy<ICache> _SqliteCacheService
            = new Lazy<ICache>(() => SmartContext.Current.Resolve<ICache>("SqliteCache"), true);

        private ConcurrentDictionary<string, CacheMetadata> _Caches
            = new ConcurrentDictionary<string, CacheMetadata>();


        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>缓存的数据</returns>
        public void Get(string key, Action<object> callback)
        {
            if (callback == null) return;
            var cacheService = GetCacheService(key);

            #region 缓存未注册
            if (cacheService == null)
            {
                callback(null);
                return;
            }
            #endregion

            var cache = cacheService.Get(key);
            if (cache == null)
            {
                #region 缓存未创建或已过期
                CacheMetadata meta;
                if (_Caches.TryGetValue(key, out meta))
                {
                    meta.GetData().ContinueWith(t =>
                    {
                        cacheService.Set(key, new CacheInfo(meta, t.Result));
                        callback(t.Result);
                    });
                }
                #endregion
            }
            else
            {
                callback(cache);
            }
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <returns>缓存的数据</returns>
        public void Get<T>(string key, Action<T> callback)
        {
            Get(key, callback);
        }

        //public bool Set(string key, object value)
        //{
        //    var cacheService = GetCacheService(key);
        //    if (cacheService == null) return false;

        //    CacheMetadata meta;
        //    if (!_Caches.TryGetValue(key, out meta))
        //    {
        //        cacheService.Set(new CacheEntry(meta, value));
        //        return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// 注册缓存
        /// </summary>
        /// <param name="cacheMetadata">缓存元数据信息，<see cref="CacheMetadata"/></param>
        public void Register(CacheMetadata cacheMetadata)
        {
            try
            {
                if (cacheMetadata == null) throw new ArgumentNullException("cacheMetadata");
                _Caches.AddOrUpdate(cacheMetadata.Key, k => cacheMetadata, (k, v) => cacheMetadata);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICache GetCacheService(string key)
        {
            CacheMetadata meta;
            if (!_Caches.TryGetValue(key, out meta))
            {
                System.Diagnostics.Debug.WriteLine("未注册缓存，Key:{0}", key);
                return null;
            }
            ICache cacheService;
            if (meta.Level == CachingLevel.First)
            {
                cacheService = _MemoryCacheService.Value;
            }
            else
            {
                cacheService = _SqliteCacheService.Value;
            }
            return cacheService;
        }

    }
}
