using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 基于当前应用程序的 System.Runtime.Caching.MemoryCache 的缓存服务
    /// </summary>
    public class MemoryCache : DisposableObject, ICache
    {
        //private static readonly Object _locker = new object();
        private static readonly System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public T Set<T>(string key, CacheInfo<T> cache) where T : class
        {
            var oldCache = _cache.Get(key);
            var cacheItem = new System.Runtime.Caching.CacheItem(key, cache.Value);
            var policy = new System.Runtime.Caching.CacheItemPolicy();
            policy.SlidingExpiration = cache.SlidingExpiration;
            _cache.Set(cacheItem, policy);
            return oldCache == null ? null : (T)oldCache;
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
        /// 从缓存中移除全部满足条件的项
        /// </summary>
        /// <param name="match">移除条件</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveAll(Predicate<string> match)
        {
            var mget = _cache.GetType().GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.NonPublic);
            var enumerator = (IEnumerator<KeyValuePair<string, object>>)mget.Invoke(_cache, null);
            while (enumerator.MoveNext())
            {
                if (match != null && match(enumerator.Current.Key))
                {
                    _cache.Remove(enumerator.Current.Key);
                }
            }
        }
    }
}
