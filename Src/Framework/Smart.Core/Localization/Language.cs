using Smart.Core.Configuration;
using Smart.Core.Extensions;
using System;
using System.IO;

namespace Smart.Core.Localization
{
    /// <summary>
    /// 多语言处理工具类
    /// </summary>
    public static class Language
    {
        /// <summary>
        /// 获取多语言字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Get(string key, params object[] args)
        {
            var config = SmartContext.Current.Resolve<SmartConfig>();
            return GetByLang(config.Language ?? "zh-CN", key, args);
        }

        /// <summary>
        /// 根据指定的语言获取
        /// </summary>
        /// <param name="lang">zh-CN ……</param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetByLang(string lang, string key, params object[] args)
        {
            var cache = SmartContext.Current.Resolve<Caching.ICache>("smart.httpCache");
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", lang + ".json");
            if (!File.Exists(filename))
            {
                return key.Contains("ξ") ? key.Split('ξ')[1] : key;
                //throw new SmartException("未找到语言文件 " + filename);
            }
            var resources = cache.Get<dynamic>(lang, () =>
            {
                using (var sr = new StreamReader(filename))
                {
                    try
                    {
                        var json = sr.ReadToEnd();
                        return json.JsonDeserialize<dynamic>();
                    }
                    catch (Exception ex)
                    {
                        throw new SmartException("语言文件格式不正确！" + filename, ex);
                    }
                }
            });
            var value = resources[key];
            if (value == null) return key.Contains("ξ") ? key.Split('ξ')[1] : key;  // 未找到语言值，直接返回 KEY
            return args == null || args.Length == 0 ? value : string.Format(value, args);
        }
    }
}
