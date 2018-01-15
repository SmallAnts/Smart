using System;
using System.ComponentModel;
using System.Reflection;

namespace Smart.Core.Extensions
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <typeparam name="T">自定义特性类型</typeparam>
        /// <param name="element"></param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
        {
#if NET40
            var attrs = element.GetCustomAttributes(typeof(T), inherit);
            return attrs.Length > 0 ? attrs[0] as T : null;
#else
            return element.GetCustomAttribute(typeof(T), inherit) as T;
#endif
        }
    }
}
