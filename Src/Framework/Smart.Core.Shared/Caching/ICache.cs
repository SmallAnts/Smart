using System;
using System.Collections.Generic;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// 获取指定键值的缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>检索到的缓存项，未找到该键时为 null。</returns>
        object Get(string key);

        /// <summary>
        /// 将指定的键和对象添加到缓存中。如果已经存在则更新缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <param name="cache">缓存对象</param>
        /// <returns>如果添加的项之前存储在缓存中，则为表示该项的对象；否则为 null。</returns>
        void Set(string key, CacheInfo cache);

        /// <summary>
        /// 从缓存中移除指定项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键值</param>
        /// <returns>从 Cache 移除的项。如果未找到键参数中的值，则返回 null。</returns>
        void Remove(string key);

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllKeys();
    }
}
