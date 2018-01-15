using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Smart.Core.Extensions
{
    public static class FiledInfoExtensions
    {
        /// <summary>
        /// 获取字段的 DisplayAttribute.Name
        /// </summary>
        /// <param name="field">字段类型</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this FieldInfo field, bool inherit = true)
        {
            var customAttribute = field.GetCustomAttribute<DisplayAttribute>(inherit);
            if (customAttribute != null)
            {
                string name = customAttribute.GetName();
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return field.Name;
        }
    }
}
