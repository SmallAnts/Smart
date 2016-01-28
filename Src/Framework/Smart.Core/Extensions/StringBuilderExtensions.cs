using System.Text;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringBuilderExtensions
    {
        private const int TrimHead = 0; // TrimStart
        private const int TrimTail = 1;    // TrimEnd
        private const int TrimBoth = 2;  // Trim

        /// <summary>
        /// 从当前 System.StringBuilder 对象移除数组中指定的一组字符的所有前导匹配项和尾部匹配项。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="trimChars">要删除的 Unicode 字符的数组，或 null。</param>
        /// <returns>从当前字符串的开头和结尾删除所出现的所有 trimChars 参数中的字符后剩余的字符串。 
        /// 如果 trimChars 为 null 或空数组，则改为移除空白字符。</returns>
        public static StringBuilder Trim(this StringBuilder stringBuilder, params char[] trimChars)
        {
            return stringBuilder.TrimHelper(TrimBoth, trimChars);
        }

        /// <summary>
        /// 从当前 System.StringBuilder 对象移除数组中指定的一组字符的所有前导匹配项。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="trimChars">要删除的 Unicode 字符的数组，或 null。</param>
        /// <returns>从当前字符串的开头移除所出现的所有 trimChars 参数中的字符后剩余的字符串。
        /// 如果 trimChars 为 null 或空数组，则改为移除空白字符。</returns>
        public static StringBuilder TrimStart(this StringBuilder stringBuilder, params char[] trimChars)
        {
            return stringBuilder.TrimHelper(TrimHead, trimChars);
        }
        /// <summary>
        /// 从当前 System.StringBuilder 对象移除数组中指定的字符串的所有前导匹配项。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="trimString">要删除的字符串</param>
        /// <returns>从当前字符串的开头移除所出现的所有 trimChars 参数字符串后剩余的字符串。</returns>
        public static StringBuilder TrimStart(this StringBuilder stringBuilder, string trimString)
        {
            if (trimString != null && trimString.Length > 0 && stringBuilder.ToString().StartsWith(trimString))
            {
                return TrimStart(stringBuilder.Remove(0, trimString.Length), trimString);
            }
            else
            {
                return stringBuilder;
            }
        }
        /// <summary>
        /// 从当前 System.StringBuilder 对象移除数组中指定的一组字符的所有尾部匹配项。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="trimChars">要删除的 Unicode 字符的数组，或 null。</param>
        /// <returns>从当前字符串的结尾移除所出现的所有 trimChars 参数中的字符后剩余的字符串。 
        /// 如果 trimChars 为 null 或空数组，则改为删除 Unicode 空白字符。</returns>
        public static StringBuilder TrimEnd(this StringBuilder stringBuilder, params char[] trimChars)
        {
            return stringBuilder.TrimHelper(TrimTail, trimChars);
        }
        /// <summary>
        /// 从当前 System.StringBuilder 对象移除数组中指定的字符串的所有尾部匹配项。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="trimString">要删除的字符串</param>
        /// <returns>从当前字符串的结尾移除所出现的所有 trimString 参数字符串后剩余的字符串。</returns>
        public static StringBuilder TrimEnd(this StringBuilder stringBuilder, string trimString)
        {
            if (trimString != null && trimString.Length > 0 && stringBuilder.ToString().EndsWith(trimString))
            {
                return TrimEnd(stringBuilder.Remove(stringBuilder.Length - trimString.Length, trimString.Length), trimString);
            }
            else
            {
                return stringBuilder;
            }
        }
        /// <summary>
        /// 从当前 System.StringBuilder 对象移除所有尾部的 "," 号。
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns>从当前字符串的结尾移除所出现的所有 "," 后剩余的字符串。</returns>
        public static StringBuilder TrimEndComma(this StringBuilder stringBuilder)
        {
            return stringBuilder.TrimHelper(TrimTail, ',');
        }

        private static StringBuilder TrimHelper(this StringBuilder stringBuilder, int trimType, params  char[] trimChars)
        {
            if (stringBuilder == null || stringBuilder.Length == 0) return stringBuilder;
            if (trimChars == null || trimChars.Length == 0) trimChars = new char[] { ' ' };

            int start = 0;
            int end = stringBuilder.Length - 1;

            if (trimType != TrimTail)
            {
                for (start = 0; start < stringBuilder.Length; start++)
                {
                    int i = 0;
                    char ch = stringBuilder[start];
                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == ch) break;
                    }
                    if (i == trimChars.Length) break;
                }
            }
            if (trimType != TrimHead)
            {
                for (end = stringBuilder.Length; end > start; end--)
                {
                    int i = 0;
                    char ch = stringBuilder[end - 1];
                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == ch) break;
                    }
                    if (i == trimChars.Length) break;
                }
            }
            return stringBuilder.CreateTrimmedStringBuilder(start, end);
        }
        private static StringBuilder CreateTrimmedStringBuilder(this StringBuilder stringBuilder, int start, int end)
        {
            int len = end - start;
            if (len == stringBuilder.Length) return stringBuilder;
            if (len == 0) return stringBuilder.Clear();

            return stringBuilder.Remove(end, stringBuilder.Length - end).Remove(0, start);
        }
    }
}
