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
        public UseCacheAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>

        public UseCacheAttribute(string key)
        {
            this.Key = key;
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
        /// 缓存相对过期时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; } = System.Web.Caching.Cache.NoSlidingExpiration;
    }

}
