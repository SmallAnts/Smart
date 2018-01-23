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
        /// 获取 类型的显示名称 ,依次从 DescriptionAttribute ，DisplayNameAttribute，DisplayAttribute 特性获取
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type, bool inherit = true)
        {
            string displayName = string.Empty;
            var attrs = type.GetCustomAttributes(typeof(DescriptionAttribute), inherit);
            if (attrs.Length > 0)
            {
                displayName = (attrs[0] as DescriptionAttribute).Description;
            }

            if (displayName.IsEmpty())
            {
                attrs = type.GetCustomAttributes(typeof(DisplayNameAttribute), inherit);
                if (attrs.Length > 0)
                {
                    displayName = (attrs[0] as DisplayNameAttribute).DisplayName;
                }
            }

            if (displayName.IsEmpty())
            {
                attrs = type.GetCustomAttributes(typeof(DisplayAttribute), inherit);
                if (attrs.Length > 0)
                {
                    displayName = (attrs[0] as DisplayAttribute).Name;
                }
            }

            return displayName.IsEmpty() ? type.Name : displayName;
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
