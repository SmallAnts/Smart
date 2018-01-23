using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Smart.Core.Extensions
{
    public static class FiledInfoExtensions
    {
        /// <summary>
        /// 获取字段的显示名称 ,依次从 DescriptionAttribute ，DisplayAttribute 特性获取
        /// </summary>
        /// <param name="field">字段类型</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns></returns>
        public static string GetDisplayName(this FieldInfo field, bool inherit = true)
        {
            string name = string.Empty;
            var descAttribute = field.GetCustomAttribute<DescriptionAttribute>(inherit);
            if (descAttribute != null)
            {
                name = descAttribute.Description;
            }
            else
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>(inherit);
                if (displayAttribute != null)
                {
                    name = displayAttribute.GetName();
                }
            }
            return string.IsNullOrEmpty(name) ? field.Name : name;
        }
    }
}
