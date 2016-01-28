using System;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 异常扩展方法
    /// </summary>
    public static class ThrowExtensions
    {
        /// <summary>
        /// 如果值为 null 则引发 ArgumentNullException 异常
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称</param>
        /// <param name="message">异常提示信息</param>
        public static void ThrowIfNull(this object argumentValue, string argumentName, string message = null)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        /// <summary>
        /// 如果值为 null 或 空白 则引发 ArgumentException 异常
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称</param>
        /// <param name="message">异常提示信息</param>
        public static void ThrowIfNullOrEmpty(this object argumentValue, string argumentName, string message = null)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
            if (string.IsNullOrEmpty(argumentName.ToString()))
            {
                throw new ArgumentException(message ?? "提供的字符串参数不能为 null 或空字符串 (\"\")。", argumentName);
            }
        }

        /// <summary>
        /// 如果值相等 则引发 Exception 异常
        /// </summary>
        /// <param name="source">参数值</param>
        /// <param name="target">参数名称</param>
        /// <param name="message">异常提示信息</param>
        public static void ThrowIfNotEqual(this object source, string target, string message)
        {
            if (source == null)
            {
                if (target != null) throw new Exception(message);
            }
            else if (!source.Equals(target))
            {
                throw new Exception(message);
            }
        }
        /// <summary>
        /// 如果 值 小于 min 或 大于 max 则引发 ArgumentOutOfRangeException 异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="argumentName">参数名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static void ThrowIfOutOfRange(this IComparable value, string argumentName, object min, object max)
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 如果对象已经释放，则引发 ObjectDisposedException 异常
        /// </summary>
        /// <param name="value">参数值</param>
        public static void ThrowIfDisposed(this DisposableObject value)
        {
            if (value.IsDisposed)
            {
                throw new ObjectDisposedException(value.GetType().Name);
            }
        }
        /// <summary>
        /// 当方法调用对于对象的当前状态无效时引发的异常。
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="predicate">条件</param>
        /// <param name="message">异常提示信息</param>
        public static void ThrowIfInvalidOperation(this object value, Func<bool> predicate, string message = null)
        {
            if (predicate != null && !predicate())
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
