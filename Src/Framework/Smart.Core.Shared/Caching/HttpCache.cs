using System;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Caching;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 基于当前应用程序的 System.Web.Caching.Cache 的缓存服务
    /// </summary>
    public class HttpCache : DisposableObject, ICache
    {
        static readonly Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <returns>检索到的缓存项，未找到该键时为 null。</returns>
        public T Get<T>(string key) where T : class
        {
            var cache = _cache.Get(key);
            return cache == null ? null : (T)cache;
        }

        /// <summary>
        /// 将对象添加到缓存，如果已经存在则更新缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <param name="cache">缓存信息</param>
        /// <returns>如果添加的项之前存储在缓存中，则为表示该项的对象；否则为 null。</returns>
        public T Set<T>(string key, CacheInfo<T> cache) where T : class
        {
            cache.Key = key;
            var oldCache = _cache.Add(key, cache.Value, null, Cache.NoAbsoluteExpiration, cache.SlidingExpiration, CacheItemPriority.Default, null);
            return oldCache == null ? null : (T)oldCache;
        }

        /// <summary>
        /// 获取一个值，该值指示是否存在指定键的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return _cache.Get(key) != null;
        }

        /// <summary>
        /// 从缓存中移除指定项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Clear()
        {
            var enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _cache.Remove(enumerator.Key.ToString());
            }
        }

        /// <summary>
        /// 从缓存中移除全部满足条件的项
        /// </summary>
        /// <param name="match">移除条件</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveAll(Predicate<string> match)
        {
            var caches = _cache.GetEnumerator();
            while (caches.MoveNext())
            {
                if (match != null && match(caches.Key.ToString()))
                {
                    _cache.Remove(caches.Key.ToString());
                }
            }
        }
    }
}
