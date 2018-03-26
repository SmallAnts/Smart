using System;
using System.Text.RegularExpressions;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 数字类型扩展方法
    /// </summary>
    public static class NumberTypeExtensions
    {
        const string FORMAT_INTEGER = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#";
        const string FORMAT_DECIMAL = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A";

        const string RMB_UPPER = "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰";
        const string RMB_LOWER = "负元空零一二三四五六七八九空空空空空空空分角十百千万亿兆京垓秭穰";
        const string CHINESE = "负元空零一二三四五六七八九空空空空空空空空十十百千万亿兆京垓秭穰";

        const string REGEX_PATTERN = @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L/.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[/.]|$))))";
        const string REGEX_REPLACEMENT = "${b}${z}";

        private static string ConvertToChinese(IFormattable number, string format, string placeholder = CHINESE)
        {
            string str = number.ToString(format, null);
            string num = Regex.Replace(str, REGEX_PATTERN, REGEX_REPLACEMENT);
            return Regex.Replace(num, ".", c => placeholder[c.Value[0] - '-'].ToString());
        }

        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase">默认大小</param>
        /// <returns></returns>
        private static string ConvertToRMB(IFormattable number, bool uppercase)
        {
            return ConvertToChinese(number, FORMAT_DECIMAL, uppercase ? RMB_UPPER : RMB_LOWER);
        }

        /// <summary>
        /// 将阿拉伯数字的金额转换为小写中文字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToChinese(this int number)
        {
            string result = string.Empty;
            if (number < 0)
            {
                result = "负";
                number = -number;
            }
            if (number >= 10 && number < 20)
            {
                return result += ConvertToChinese(number, FORMAT_INTEGER).Substring(1);
            }
            else
            {
                return result += ConvertToChinese(number, FORMAT_INTEGER);
            }
        }
        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ToRMB(this int number, bool uppercase = true)
        {
            if (number < 0)
            {
                return "负" + ConvertToRMB(-number, uppercase);
            }
            else
            {
                return ConvertToRMB(number, uppercase);
            }
        }

        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ToRMB(this long number, bool uppercase = true)
        {
            if (number < 0)
            {
                return "负" + ConvertToRMB(-number, uppercase);
            }
            else
            {
                return ConvertToRMB(number, uppercase);
            }
        }
        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ToRMB(this float number, bool uppercase = true)
        {
            if (number < 0)
            {
                return "负" + ConvertToRMB(-number, uppercase);
            }
            else
            {
                return ConvertToRMB(number, uppercase);
            }
        }
        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ToRMB(this double number, bool uppercase = true)
        {
            if (number < 0)
            {
                return "负" + ConvertToRMB(-number, uppercase);
            }
            else
            {
                return ConvertToRMB(number, uppercase);
            }
        }
        /// <summary>
        /// 将阿拉伯数字的金额转换为中文人民币字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ToRMB(this decimal number, bool uppercase = true)
        {
            if (number < 0)
            {
                return "负" + ConvertToRMB(-number, uppercase);
            }
            else
            {
                return ConvertToRMB(number, uppercase);
            }
        }
    }
}
