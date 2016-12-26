using Smart.Core.Caching;
using System;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 缓存相关扩展方法
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取缓存项目。如果缓存不存在则通过acquire方法加载对象并缓存,默认超时时间为5分钟
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键</param>
        /// <param name="acquire">当缓存不存在时的缓存对象获取方法</param>
        /// <returns>缓存对象</returns>
        public static T Get<T>(this ICache cacheManager, string key, Func<T> acquire) where T : class
        {
            return Get(cacheManager, key, new TimeSpan(0, 5, 0), acquire);
        }

        /// <summary>
        /// 获取缓存项目。如果缓存不存在则通过acquire方法加载对象并缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键</param>
        /// <param name="slidingExpiration">相对缓存超时时间</param>
        /// <param name="acquire">当缓存不存在时的缓存对象获取方法</param>
        /// <returns>缓存对象</returns>
        public static T Get<T>(this ICache cacheManager, string key, TimeSpan slidingExpiration, Func<T> acquire) where T : class
        {
            if (cacheManager.Exists(key))
            {
                return cacheManager.Get<T>(key);
            }
            var result = acquire();
            if (slidingExpiration == null)
            {
                cacheManager.Set(key, new CacheInfo<T>(result));
            }
            else
            {
                cacheManager.Set(key, new CacheInfo<T>(slidingExpiration, result));
            }
            return result;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键值</param>
        /// <param name="data">缓存对象</param>
        public static void Set<T>(this ICache cacheManager, string key, T data) where T : class
        {
            cacheManager.Set<T>(key, new CacheInfo<T>(data));
        }

        /// <summary>
        /// 设置缓存及缓存超时时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键值</param>
        /// <param name="data">缓存对象</param>
        /// <param name="slidingExpiration">相对超时时间</param>
        public static void Set<T>(this ICache cacheManager, string key, T data, TimeSpan slidingExpiration) where T : class
        {
            cacheManager.Set<T>(key, new CacheInfo<T>(slidingExpiration, data));
        }

    }
}
