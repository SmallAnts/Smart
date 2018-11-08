using System;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 启用缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UseCacheAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public UseCacheAttribute() : this(null, CachingLevel.First)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>

        public UseCacheAttribute(string key) : this(key, CachingLevel.First)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="level"></param>

        public UseCacheAttribute(string key, CachingLevel level)
        {
            this.Key = key;
            this.Level = level;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// 缓存键值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存级别
        /// </summary>
        public CachingLevel Level { get; set; }

        /// <summary>
        /// 缓存相对过期时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; } = System.Web.Caching.Cache.NoSlidingExpiration;
    }

}
