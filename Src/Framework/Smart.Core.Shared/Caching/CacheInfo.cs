using System;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 缓存信息
    /// </summary>
    public class CacheInfo
    {
        //public CacheInfo() { }
        public CacheInfo(string key, object value)
           : this(key, value, null, null)
        {
        }
        public CacheInfo(string key, object value, TimeSpan? slidingExp)
            : this(key, value, slidingExp, null)
        {
        }
        public CacheInfo(string key, object value, DateTime? absExp)
            : this(key, value, null, absExp)
        {
        }
        internal CacheInfo(CacheMetadata meta, object value)
             : this(meta.Key, value, meta.SlidingExpiration, meta.AbsoluteExpiration)
        {
        }
        internal CacheInfo(string key, object value, TimeSpan? slidingExp, DateTime? absExp)
        {
            this.Key = key;
            this.Value = value;
            this.Created = DateTime.Now;
            this.SlidingExpiration = slidingExp ?? System.Web.Caching.Cache.NoSlidingExpiration;
            this.AbsoluteExpiration = absExp ?? System.Web.Caching.Cache.NoAbsoluteExpiration;
            this.LastUpdateUsage = DateTime.Now;
        }
        /// <summary>
        /// 获取缓存键值
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        //[Browsable(false),
        //EditorBrowsable(EditorBrowsableState.Never),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value { get; set; }

        /// <summary>
        /// 缓存级别
        /// </summary>
        public CachingLevel Level { get; set; }

        /// <summary>
        /// 缓存创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastUpdateUsage { get; set; }

        /// <summary>
        /// 获取或设置相对过期时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; } = System.Web.Caching.Cache.NoSlidingExpiration;

        /// <summary>
        ///  获取或设置绝对过期时间
        /// </summary>
        public DateTime AbsoluteExpiration { get; set; } = System.Web.Caching.Cache.NoAbsoluteExpiration;

    }
}
