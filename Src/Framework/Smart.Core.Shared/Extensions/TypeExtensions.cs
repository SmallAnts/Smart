using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// Type 扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取 DisplayName
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type, bool inherit = true)
        {

            var attrs = type.GetCustomAttributes(typeof(DisplayNameAttribute), inherit);
            if (attrs.Length > 0)
            {
                return (attrs[0] as DisplayNameAttribute).DisplayName ?? type.Name;
            }
            else
            {
                attrs = type.GetCustomAttributes(typeof(DisplayAttribute), inherit);
                return attrs.Length > 0 ? (attrs[0] as DisplayAttribute).Name ?? type.Name : type.Name;
            }
        }

        /// <summary>
        /// 判断类型是否为可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// 判断对象的类型是否为可空类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullableType<T>(this T obj)
        {
            var type = typeof(T);
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}
