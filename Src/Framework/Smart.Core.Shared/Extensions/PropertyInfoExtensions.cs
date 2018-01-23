using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 获取属性的显示名称 ,依次从 DescriptionAttribute ，DisplayNameAttribute，DisplayAttribute 特性获取
        /// </summary>
        /// <param name="property"></param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this PropertyInfo property, bool inherit = true)
        {
            string displayName = string.Empty;
            var attrs = property.GetCustomAttributes(typeof(DescriptionAttribute), inherit);
            if (attrs.Length > 0)
            {
                displayName = (attrs[0] as DescriptionAttribute).Description;
            }

            if (string.IsNullOrEmpty(displayName))
            {
                attrs = property.GetCustomAttributes(typeof(DisplayNameAttribute), inherit);
                if (attrs.Length > 0)
                {
                    displayName = (attrs[0] as DisplayNameAttribute).DisplayName;
                }
            }

            if (string.IsNullOrEmpty(displayName))
            {
                attrs = property.GetCustomAttributes(typeof(DisplayAttribute), inherit);
                if (attrs.Length > 0)
                {
                    displayName = (attrs[0] as DisplayAttribute).Name;
                }
            }

            return string.IsNullOrEmpty(displayName) ? property.Name : displayName;
        }
    }
}
