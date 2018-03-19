using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 连接字符串扩展方法
    /// </summary>
    public static class ConnectionStringSettingsCollectionExtensions
    {
        /// <summary>
        /// 动态修改连接字符串
        /// </summary>
        /// <param name="name">连接字符串名</param>
        /// <param name="connectionString">连接字符串值</param>
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static void Update(this ConnectionStringSettingsCollection connectionStrings, string name, string connectionString)
        {
            var conn = connectionStrings[name];
            if (conn == null) return; // 连接名不存在

            #region 通过反射执行 _values.GetConfigValue("connectionString").Value = connectionString;

            var _valuesField = typeof(ConfigurationElement).GetField("_values",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var _values = _valuesField.GetValue(conn);

            var getConfigValue = _values.GetType().GetMethod("GetConfigValue",
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
            var configValue = getConfigValue.Invoke(_values, new object[] { "connectionString" });

            var setConfigValue = configValue.GetType().GetField("Value",
                BindingFlags.Instance | BindingFlags.NonPublic);
            setConfigValue.SetValue(configValue, connectionString);

            #endregion

        }
    }
}
