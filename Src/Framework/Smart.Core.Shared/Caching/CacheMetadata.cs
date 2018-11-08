using System;
using System.Threading.Tasks;

namespace Smart.Core.Caching
{
    public class CacheMetadata
    {
        public CacheMetadata(string key) : this(key, CachingLevel.First)
        {
        }
        public CacheMetadata(string key, CachingLevel level)
        {
            this.Key = key;
            this.Level = level;
        }

        public string Key { get; set; }
        public CachingLevel Level { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public DateTime? AbsoluteExpiration { get; set; }

        public Func<Task<object>> GetData { get; set; }
    }
}
