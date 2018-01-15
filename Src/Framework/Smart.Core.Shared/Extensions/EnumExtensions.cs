using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>  
        /// 添加一个或多个枚举值，枚举类型必须设置 [Flags]
        /// </summary>  
        public static TEnum SetFlag<TEnum>(this Enum @enum, TEnum values)
        {
            var enumType = @enum.GetType();
            var valueType = values.GetType();
            if (enumType != valueType)
                throw new ArgumentException("参数类型“" + enumType.FullName + "”与枚举类型“" + valueType.FullName + "”不相同。");
            if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false)?.Length > 0)
            {
                return (TEnum)Enum.ToObject(enumType, Convert.ToInt32(@enum) | Convert.ToInt32(values));
            }
            else if (Enum.IsDefined(enumType, values))
            {
                return values;
            }
            else
            {
                throw new ArgumentException("枚举类型“" + enumType.FullName + "”不包含值为 " + values + " 的定义。");
            }
        }

        public enum Sex
        {

        }
        /// <summary>  
        /// 移除一个或多个枚举值，枚举类型必须设置 [Flags]
        /// </summary>  
        public static TEnum DelFlag<TEnum>(this Enum @enum, TEnum values)
        {
            var enumType = @enum.GetType();
            var valueType = values.GetType();
            if (enumType != valueType)
                throw new ArgumentException("values", "参数类型“" + enumType.FullName + "”与枚举类型“" + valueType.FullName + "”不相同。");

            return (TEnum)Enum.ToObject(enumType, Convert.ToInt32(@enum) & ~Convert.ToInt32(values));
        }


        /// <summary>
        /// 将枚举对象转换为值和文本的对象列表
        /// </summary>
        /// <param name="enum">枚举类型</param>
        /// <param name="intValue">intValue=true,值为int类型, 否则为枚举类型</param>
        /// <returns></returns>
        public static List<Infrastructure.BaseItem<object>> ToValueNameList(this Enum @enum, bool intValue = true)
        {
            var enumType = @enum.GetType();
            var list = new List<Infrastructure.BaseItem<object>>();
            var fields = enumType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo fieldInfo = fields[i];
                list.Add(new Infrastructure.BaseItem<object>
                {
                    Text = fieldInfo.GetDisplayName(),
                    Value = intValue ? fieldInfo.GetRawConstantValue() : fieldInfo.GetValue(@enum)
                });
            }
            return list;
        }


    }

}
