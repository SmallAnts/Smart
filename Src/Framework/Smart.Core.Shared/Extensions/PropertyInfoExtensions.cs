using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Smart.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 获取 属性的 DisplayName
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this PropertyInfo property, bool inherit = true)
        {
            var attrs = property.GetCustomAttributes(typeof(DisplayNameAttribute), inherit);
            if (attrs.Length > 0)
            {
                return (attrs[0] as DisplayNameAttribute).DisplayName ?? property.Name;
            }
            else
            {
                attrs = property.GetCustomAttributes(typeof(DisplayAttribute), inherit);
                return attrs.Length > 0 ? (attrs[0] as DisplayAttribute).Name ?? property.Name : property.Name;
            }
        }
    }
}
