using System.Collections.Generic;
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
        private static readonly Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>检索到的缓存项，未找到该键时为 null。</returns>
        public object Get(string key)
        {
            var cache = _cache.Get(key);
            return cache;
        }

        /// <summary>
        /// 将对象添加到缓存，如果已经存在则更新缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="value">缓存信息</param>
        /// <returns>如果添加的项之前存储在缓存中，则为表示该项的对象；否则为 null。</returns>
        void ICache.Set(string key, CacheInfo value)
        {
            _cache.Add(key, value.Value, null, value.AbsoluteExpiration, value.SlidingExpiration, CacheItemPriority.Default, null);
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
        public IEnumerable<string> GetAllKeys()
        {
            var caches = _cache.GetEnumerator();
            while (caches.MoveNext())
            {
                yield return caches.Key.ToString();
            }
        }

    }
}
