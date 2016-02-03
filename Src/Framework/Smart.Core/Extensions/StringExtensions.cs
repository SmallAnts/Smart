using System;
using System.Text.RegularExpressions;
using Smart.Core.Utilites;
using Newtonsoft.Json;
using System.Globalization;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        #region 字符检查 Is

        /// <summary>检查字符串的值是否为null或空。</summary>
        /// <param name="value">检查字符串的值.</param>
        /// <returns>true  如果 <paramref name="value" />为null或空字符串; 否则, false.</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        /// <summary>检查一个字符串是否可以转换为布尔值（真或假）型。</summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        public static bool IsBool(this string value)
        {
            bool flag;
            return bool.TryParse(value, out flag);
        }

        /// <summary>检查一个字符串是否可以转换为整形。</summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        public static bool IsInt(this string value)
        {
            int num;
            return int.TryParse(value, out num);
        }

        /// <summary>检查一个字符串是否可以转换为 <see cref="T:System.Decimal" /> 类型.</summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        public static bool IsDecimal(this string value)
        {
            return value.Is<decimal>();
        }

        /// <summary>检查一个字符串是否可以转换为 <see cref="T:System.Single" /> 类型.</summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        public static bool IsFloat(this string value)
        {
            float num;
            return float.TryParse(value, out num);
        }

        /// <summary>检查一个字符串是否可以转换为 <see cref="T:System.DateTime" /> 类型.</summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 可以转换为指定的类型; 否则, false.</returns>
        public static bool IsDateTime(this string value)
        {
            DateTime dateTime;
            return DateTime.TryParse(value, out dateTime);
        }

        /// <summary>
        /// 检查一个字符串是否由中文组成
        /// </summary>
        /// <param name="value">检查字符串值。</param>
        /// <returns>true 如果 <paramref name="value" /> 全是中文; 否则, false.</returns>
        public static bool IsChinese(this string value)
        {
            return Regex.IsMatch(value, @"^[\u4e00-\u9fa5]+$");
        }
        /// <summary>
        /// 是否是邮编格式字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZip(this string value)
        {
            return Regex.IsMatch(value, @"^[1-9]\d{5}$");
        }
        /// <summary>
        /// 是否是数字字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this string value)
        {
            return Regex.IsMatch(value, @"^[0-9]+$");
        }
        /// <summary>
        /// 验证字符串EMAIL地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            return Regex.IsMatch(value, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 验证字符串URL地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(this string value)
        {
            return Regex.IsMatch(value, @"[a-zA-z]+://[^\s]*"); //^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$
        }

        /// <summary>
        /// 验证字符串是否是数值类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumericString(string value)
        {
            double result;
            return (double.TryParse(value, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result));
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项。
        /// </summary>
        /// <param name="value">输入字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="options">匹配选项</param>
        /// <returns></returns>
        public static bool IsMatch(this string value, string regex, RegexOptions options = RegexOptions.Singleline)
        {
            return Regex.IsMatch(value, regex, options);
        }
        #endregion

        #region 类型转换 As

        /// <summary>将字符串转换为整数。</summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static int AsInt(this string value)
        {
            return value.AsInt(0) ?? 0;
        }

        /// <summary>将字符串转换为整数，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static int? AsInt(this string value, int? defaultValue)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为整数。</summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static long AsLong(this string value)
        {
            return value.AsLong(0) ?? 0L;
        }

        /// <summary>将字符串转换为整数，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static long? AsLong(this string value, long? defaultValue)
        {
            long result;
            if (!long.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为<see cref="T:System.Decimal" /></summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static decimal AsDecimal(this string value)
        {
            return value.AsDecimal(0).Value;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Decimal" /> ，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static decimal? AsDecimal(this string value, decimal? defaultValue)
        {
            decimal result;
            if (!decimal.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static float AsFloat(this string value)
        {
            return value.AsFloat(0f).Value;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> ，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static float? AsFloat(this string value, float? defaultValue)
        {
            float result;
            if (!float.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Double" /> </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static double AsDouble(this string value)
        {
            return value.AsDouble(0f).Value;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Double" /> ，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static double? AsDouble(this string value, double? defaultValue)
        {
            double result;
            if (!double.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /></summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static DateTime AsDateTime(this string value)
        {
            return value.AsDateTime(default(DateTime)).Value;
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /> ，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static DateTime? AsDateTime(this string value, DateTime? defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Boolean" /></summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的值。</returns>
        public static bool AsBool(this string value)
        {
            return value.AsBool(false).Value;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Boolean" />，并指定一个默认值。</summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">如果 <paramref name="value" /> 为空或是一个无效的值则返回该值,。</param>
        /// <returns>转换后的值。</returns>
        public static bool? AsBool(this string value, bool? defaultValue)
        {
            bool result;
            if (!bool.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        #endregion

        #region 单词转换 To

        /// <summary>
        /// 每个单词首字母大写
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string word)
        {
            return Regex.Replace(word, @"\b([a-z])", m => m.Captures[0].Value.ToUpper());
        }

        /// <summary>
        /// 将单词转换为首字母大写形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns></returns>
        public static string ToInitialUppercase(this string word)
        {
            return String.Concat(word.Substring(0, 1).ToUpper(), word.Substring(1).ToLower());
        }

        /// <summary>
        /// 将单词转换为首字母小写形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToInitialLowercase(this string word)
        {
            return String.Concat(word.Substring(0, 1).ToLower(), word.Substring(1));
        }

        /// <summary>
        /// 获取字符串的五笔码
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的字符串。</returns>
        public static string ToFivePenCode(this string value)
        {
            return InputCodeUtility.GetWuBiMa(value);
        }

        /// <summary>
        /// 获取字符串的首字母简拼
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的字符串。</returns>
        public static string ToAcronyms(this string value)
        {
            return InputCodeUtility.GetFirstPinYin(value);
        }

        /// <summary>
        /// 获取汉字字符串的全拼形式
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <returns>转换后的字符串。</returns>
        public static string ToCompleteSpellings(this string value)
        {
            return InputCodeUtility.GetPinYin(value);
        }

        #endregion
        
        /// <summary>
        /// JSON 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 以该字符串为键值获取本地化语言。
        /// </summary>
        /// <param name="key">语言键值</param>
        /// <param name="args">格式化参数</param>
        /// <returns></returns>
        public static string T(this string key, params object[] args)
        {
            return Localization.Language.Get(key, args);
        }
    }
}
