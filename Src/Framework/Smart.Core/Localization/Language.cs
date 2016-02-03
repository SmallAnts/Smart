using Smart.Core.Configuration;
using Smart.Core.Extensions;
using System;
using System.IO;

namespace Smart.Core.Localization
{
    public static class Language
    {
        public static string Get(string key, params object[] args)
        {
            var config = SmartContext.Current.Resolve<SmartConfig>();
            return GetByLang(config.Language ?? "zh-CN", key, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang">zh-CN ……</param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetByLang(string lang, string key, params object[] args)
        {
            var cache = SmartContext.Current.Resolve<Caching.ICache>("smart.lang");
            var resources = cache.Get<dynamic>(lang, () =>
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", lang + ".json");
                if (!File.Exists(filename)) throw new SmartException("未找到语言文件 " + filename);
                using (var sr = new StreamReader(filename))
                {
                    try
                    {
                        var json = sr.ReadToEnd();
                        return json.DeserializeObject<dynamic>();
                    }
                    catch (Exception ex)
                    {
                        throw new SmartException("语言文件格式不正确！" + filename, ex);
                    }
                }
            });
            var value = resources[key];
            if (value == null) return key; // 未找到语言值，直接返回 KEY
            return args == null || args.Length == 0 ? value : string.Format(value, args);
        }
    }
}
