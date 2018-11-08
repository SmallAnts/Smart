using Smart.Core.Caching;
using System;
using System.Runtime.CompilerServices;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 缓存相关扩展方法
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取指定键值的缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <returns>检索到的缓存项，未找到该键时为 null。</returns>
        public static T Get<T>(this ICache cache, string key) where T : class
        {
            var value = cache.Get(key);
            if (value is string && typeof(T) != typeof(string))
            {
                return value.AsString().JsonTo<T>();
            }
            return value as T;
        }

        /// <summary>
        /// 获取缓存项目。如果缓存不存在则通过acquire方法加载对象并缓存,默认超时时间为5分钟
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key">缓存键</param>
        /// <param name="acquire">当缓存不存在时的缓存对象获取方法</param>
        /// <returns>缓存对象</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T Get<T>(this ICache cache, string key, Func<T> acquire) where T : class
        {
            return Get(cache, key, new TimeSpan(0, 5, 0), acquire);
        }

        /// <summary>
        /// 获取缓存项目。如果缓存不存在则通过acquire方法加载对象并缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cache"></param>
        /// <param name="key">缓存键</param>
        /// <param name="slidingExpiration">相对缓存超时时间</param>
        /// <param name="acquire">当缓存不存在时的缓存对象获取方法</param>
        /// <returns>缓存对象</returns>
        public static T Get<T>(this ICache cache, string key, TimeSpan slidingExpiration, Func<T> acquire) where T : class
        {
            if (cache.Exists(key))
            {
                return cache.Get<T>(key);
            }
            if (acquire == null) return null;

            var value = acquire();
            if (slidingExpiration == null || slidingExpiration == System.Web.Caching.Cache.NoSlidingExpiration)
            {
                cache.Set(key, new CacheInfo(key, value));
            }
            else
            {
                cache.Set(key, new CacheInfo(key, value, slidingExpiration));
            }
            return value;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key">缓存键值</param>
        /// <param name="value">缓存对象</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Set<T>(this ICache cache, string key, T value) where T : class
        {
            if (value is CacheInfo)
            {
                cache.Set(key, value as CacheInfo);
            }
            else
            {
                cache.Set(key, new CacheInfo(key, value));
            }
        }

        /// <summary>
        /// 设置缓存及缓存超时时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key">缓存键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="slidingExpiration">相对超时时间</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Set<T>(this ICache cache, string key, T value, TimeSpan slidingExpiration) where T : class
        {
            cache.Set(key, new CacheInfo(key, value, slidingExpiration));
        }

        /// <summary>
        /// 获取一个值，该值指示是否存在指定键的缓存对象
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(this ICache cache, string key)
        {
            return cache.Get(key) != null;
        }

        /// <summary>
        /// 从缓存中移除全部满足条件的项
        /// </summary>
        /// <param name="match">移除条件</param>
        public static void RemoveAll(this ICache cache, Predicate<string> match)
        {
            foreach (var key in cache.GetAllKeys())
            {
                if (match == null || match(key))
                {
                    cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <param name="cacheManager"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Clear(this ICache cacheManager)
        {
            cacheManager.RemoveAll(k => true);
        }


        //public static object Get(this CacheManager cm, string key) {
        //    cm.Get(key, () => {

        //    });
        //}
    }
}
