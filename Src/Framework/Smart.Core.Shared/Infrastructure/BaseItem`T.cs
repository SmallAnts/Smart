using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 包含一个 int 值和一个 string 文本的对象
    /// </summary>
    public class BaseItem : BaseItem<int>
    {
    }

    /// <summary>
    /// 包含一个值和文本的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseItem<T>
    {
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
    }
}
