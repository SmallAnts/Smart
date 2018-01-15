using System;
using System.Web.Caching;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 缓存对象信息
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    public class CacheInfo<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">被缓存的对象</param>
        public CacheInfo(T value) : this(Cache.NoSlidingExpiration, value)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <param name="value">被缓存的对象</param>
        public CacheInfo(TimeSpan slidingExpiration, T value)
        {
            this.SlidingExpiration = slidingExpiration;
            this.Value = value;
        }

        /// <summary>
        /// 获取缓存键值
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// 获取或设置缓存项
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 获取或设置相对过期时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; }

    }
}
