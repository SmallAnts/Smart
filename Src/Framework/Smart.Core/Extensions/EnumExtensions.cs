using System;

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

    }
}
