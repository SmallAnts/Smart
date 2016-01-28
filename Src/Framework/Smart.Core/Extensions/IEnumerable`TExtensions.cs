using System;
using System.Collections.Generic;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 集合类型扩展方法
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 返回按指定字符分隔的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listValue"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> listValue, string separator = ",")
        {
            return string.Join(separator, listValue);
        }

        /// <summary>
        /// 将类型集合转换为类型实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToInstances<T>(this IEnumerable<Type> types)
        {
            foreach (var drType in types)
            {
                if (!drType.IsInterface)
                    yield return (T)Activator.CreateInstance(drType);
            }
        }
    }
}
