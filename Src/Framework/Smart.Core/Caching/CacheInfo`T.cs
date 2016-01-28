using System;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 缓存对象信息
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    public class CacheInfo<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public CacheInfo(T value)
        {
            this.SlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slidingExpiration"></param>
        /// <param name="value"></param>
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
