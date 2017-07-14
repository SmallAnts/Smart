using System;
using System.Text.RegularExpressions;
using Smart.Core.Utilites;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.Compression;

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

        #region 字符串压缩

        /// <summary>  
        /// 对字符串进行压缩  
        /// </summary>  
        /// <param name="str">待压缩的字符串</param>  
        /// <returns>压缩后的字符串</returns>  
        public static string Compress(this string str)
        {
            string compressString = string.Empty;
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            byte[] compressAfterByte = Compress(compressBeforeByte);
            compressString = Convert.ToBase64String(compressAfterByte, Base64FormattingOptions.None);
            return compressString;
        }

        /// <summary>  
        /// 对字符串进行解压缩  
        /// </summary>  
        /// <param name="str">待解压缩的字符串</param>  
        /// <returns>解压缩后的字符串</returns>  
        public static string Decompress(this string str)
        {
            string compressString = string.Empty;
            byte[] compressBeforeByte = Convert.FromBase64String(str);
            byte[] compressAfterByte = Decompress(compressBeforeByte);
            compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return compressString;
        }

        private static byte[] Compress(byte[] data)
        {
            byte[] buffer = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new DeflateStream(ms, CompressionMode.Compress, true))
                    {
                        zip.Write(data, 0, data.Length);
                    }
                    buffer = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(buffer, 0, buffer.Length);
                }
                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static byte[] Decompress(byte[] data)
        {
            try
            {
                byte[] buffer = null;
                using (var ms = new MemoryStream(data))
                {
                    using (var zip = new DeflateStream(ms, CompressionMode.Decompress, true))
                    {
                        buffer = new byte[0x1000];
                        using (var msreader = new MemoryStream())
                        {
                            int len = 0;
                            while ((len = zip.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                msreader.Write(buffer, 0, len);
                            }
                            msreader.Position = 0;
                            buffer = msreader.ToArray();
                        }
                    }
                }
                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region JSON序列化 基于 Newtonsoft.Json

        /// <summary>
        /// JSON 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string json)
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
        #endregion

        #region 散列加密算法

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MD5Encrypt(this string value)
        {
            return CryptoUtility.MD5Encrypt(value);
        }

        /// <summary>
        /// SHA256 散列算法加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SHA256Encrypt(this string value)
        {
            return CryptoUtility.SHA256Encrypt(value);
        }

        #endregion

        #region 对称加密算法

        /// <summary>
        /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合）
        /// </summary>
        /// <param name="encryptString">待加密的明文</param>
        /// <param name="encryptKey">加密的密钥,必须为8位</param>
        /// <returns>加密后的密文</returns>
        public static string DESEncrypt(this string encryptString, string encryptKey)
        {
            return CryptoUtility.DESEncrypt(encryptString, encryptKey);
        }


        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合）
        /// </summary>
        /// <param name="decryptString">待解密的密文</param>
        /// <param name="decryptKey">解密的密钥,必须为8位</param>
        /// <returns>解密后的明文</returns>
        public static string DESDecrypt(this string decryptString, string decryptKey)
        {
            return CryptoUtility.DESDecrypt(decryptString, decryptKey);
        }

        /// <summary>
        /// RC2 加密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Encrypt(this string encryptString, string encryptKey)
        {
            return CryptoUtility.RC2Encrypt(encryptString, encryptKey);
        }

        /// <summary>
        /// RC2 解密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Decrypt(this string decryptString, string decryptKey)
        {
            return CryptoUtility.RC2Decrypt(decryptString, decryptKey);
        }

        /// <summary>
        /// 3DES 加密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey1">密钥一</param>
        /// <param name="encryptKey2">密钥二</param>
        /// <param name="encryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Encrypt(this string encryptString, string encryptKey1, string encryptKey2, string encryptKey3)
        {
            return CryptoUtility.DES3Encrypt(encryptString, encryptKey1, encryptKey2, encryptKey3);
        }

        /// <summary>
        /// 3DES 解密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey1">密钥一</param>
        /// <param name="decryptKey2">密钥二</param>
        /// <param name="decryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Decrypt(this string decryptString, string decryptKey1, string decryptKey2, string decryptKey3)
        {
            return CryptoUtility.DES3Decrypt(decryptString, decryptKey1, decryptKey2, decryptKey3);
        }

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(this string encryptString, string encryptKey)
        {
            return CryptoUtility.AESEncrypt(encryptString, encryptKey);
        }

        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(this string decryptString, string decryptKey)
        {
            return CryptoUtility.AESDecrypt(decryptString, decryptKey);
        }

        #endregion

        #region 非对称加密算法

        /// <summary>
        /// RSA 非对称加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RSAEncrypt(this string data, string key)
        {
            return CryptoUtility.RSAEncrypt(data, key);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RSADecrypt(this string data, string key)
        {
            return CryptoUtility.RSADecrypt(data, key);
        }
        #endregion
    }
}
