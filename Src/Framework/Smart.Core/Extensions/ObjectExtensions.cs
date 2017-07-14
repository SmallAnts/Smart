using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Smart.Core.Data;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 对象扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>将值转换为指定的数据类型的值。</summary>
        /// <typeparam name="TValue">转换的数据类型。</typeparam>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static TValue As<TValue>(this object value)
        {
            return value.As(default(TValue));
        }

        /// <summary>将值转换为指定的数据类型和指定默认值。</summary>
        /// <typeparam name="TValue">转换的数据类型。</typeparam>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static TValue As<TValue>(this object value, TValue defaultValue)
        {
            if (value == null || value is DBNull) return defaultValue;
            try
            {
                var targetType = typeof(TValue);
                var valueType = value.GetType();
                // 类型相同
                if (valueType == targetType)
                {
                    return (TValue)value;
                }
                if (targetType.IsEnum)
                {
                    if (valueType == typeof(int))
                    {
                        return (TValue)Enum.ToObject(targetType, value);
                    }
                    else
                    {
                        return (TValue)Enum.Parse(targetType, value.ToString());
                    }
                }
                // 类型是否可以从对象转换
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(value.GetType()))
                {
                    TValue result = (TValue)converter.ConvertFrom(value);
                    return result;
                }
                // 值是否可以转换为指定的类型
                converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(targetType))
                {
                    TValue result = (TValue)converter.ConvertTo(value, targetType);
                    return result;
                }
            }
            catch
            {
            }
            return defaultValue;
        }

        /// <summary>
        /// 将值转换为字符串
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>返回表示当前对象的字符串，如果 <paramref name="value" /> 为null,则返回string.Empty</returns>
        public static string AsString(this object value)
        {
            return value == null || value is DBNull ? string.Empty : value.ToString();
        }

        /// <summary>检查一个值是否可以转换为指定的数据类型。</summary>
        /// <typeparam name="TValue">要转换的数据类型。</typeparam>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        /// <param name="value">检查字符串值。</param>
        public static bool Is<TValue>(this object value)
        {
            if (value == null && typeof(TValue).IsClass) return true;
            var converter = TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null)
            {
                try
                {
                    if (value == null || converter.CanConvertFrom(null, value.GetType()))
                    {
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过属性名称动态给属性赋值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public static void Set<TValue>(this IEntity obj, string name, TValue value)
        {
            var cache = SmartContext.Current.Resolve<Caching.ICache>(SmartContext.DEFAULT_CACHE_KEY);
            var type = obj.GetType();
            var pis = cache.Get(type.FullName, () => type.GetProperties());
            var pi = pis.FirstOrDefault(p => p.Name == name);
            if (pi == null)
            {
                throw new ArgumentException("name", $"{name}属性不存在。");
            }
            var valueType = value.GetType();
            // 类型相同，或和可空类型中的类型相同，不需要类型转换
            if (valueType == pi.PropertyType)// || Nullable.GetUnderlyingType(pi.PropertyType) == valueType
            {
                pi.SetValue(obj, value, null);
                return;
            }
            // 类型是否可以从对象转换
            var converter = TypeDescriptor.GetConverter(pi.PropertyType);
            if (converter.CanConvertFrom(value.GetType()))
            {
                pi.SetValue(obj, converter.ConvertFrom(value), null);
                return;
            }
            // 值是否可以转换为指定的类型
            converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertTo(pi.PropertyType))
            {
                pi.SetValue(obj, converter.ConvertTo(value, pi.PropertyType), null);
                return;
            }
            throw new ArgumentException("value", $"{value.GetType().FullName}和{pi.PropertyType.FullName}类型不一致。");
        }

        /// <summary>
        /// 通过属性名称获取属性值。
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static object Get(this IEntity obj, string name)
        {
            var cache = SmartContext.Current.Resolve<Caching.ICache>(SmartContext.DEFAULT_CACHE_KEY);
            var type = obj.GetType();
            var pis = cache.Get(type.FullName, () => type.GetProperties());
            var pi = pis.FirstOrDefault(p => p.Name == name);
            if (pi == null)
            {
                throw new ArgumentException("name", $"{name}属性不存在。");
            }
            return pi.GetValue(obj, null);
        }
        /// <summary>
        /// 将对象序列化为JSON字符串，循环引用的对象将被忽略
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <param name="nullValue"></param>
        /// <param name="referenceLoop"></param>
        /// <returns></returns>
        public static string ToJson(this object value,
            Formatting formatting = Formatting.None,
            NullValueHandling nullValue = NullValueHandling.Ignore,
            ReferenceLoopHandling referenceLoop = ReferenceLoopHandling.Ignore,
            JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = referenceLoop, // 忽略循环引用
                    NullValueHandling = nullValue,
                };
            }
            var json = JsonConvert.SerializeObject(value, formatting, settings);
            return json;
        }
#if NET45
        /// <summary>
        ///  将对象序列化为JSON字符串，循环引用的对象将被忽略
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static async Task<string> ToJsonAsync(this object value, Formatting formatting = Formatting.None, JsonSerializerSettings settings = null)
        {
            var task = Task.Factory.StartNew(() =>
            {
                if (settings == null)
                {
                    settings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 忽略循环引用
                        NullValueHandling = NullValueHandling.Ignore,
                    };
                }
                return JsonConvert.SerializeObject(value, formatting, settings);
            });
            return await task;
        }

#endif

        /// <summary>
        /// 深拷贝对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Copy<T>(this T obj)
        {
            return obj.ToJson().JsonDeserialize<T>();
        }
    }
}
