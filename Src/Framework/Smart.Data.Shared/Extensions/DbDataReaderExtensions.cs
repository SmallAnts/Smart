using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;

namespace Smart.Data.Extensions
{
    /// <summary>
    /// DbDataReader 扩展方法类
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// 将 DbDataReader 转换为 dynamic 对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static dynamic AsDynamic(this DbDataReader reader)
        {
            var entity = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < reader.FieldCount; fieldCount++)
            {
                entity.Add(reader.GetName(fieldCount), reader[fieldCount]);
            }
            return entity;
        }
    }
}
