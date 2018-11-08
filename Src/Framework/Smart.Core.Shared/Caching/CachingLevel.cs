using System;

namespace Smart.Core.Caching
{
    /// <summary>
    /// 缓存级别
    /// </summary>
    [Flags]
    public enum CachingLevel
    {
        /// <summary>
        /// 一级，内存
        /// </summary>
        First = 1,

        /// <summary>
        /// 二级，硬盘
        /// </summary>
        Second = 2
    }
}
