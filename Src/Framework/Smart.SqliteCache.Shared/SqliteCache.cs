using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Smart.Core.Caching
{
    public class SqliteCache : DisposableObject, ICache
    {
        private Database database;

        public SqliteCache()
        {
            database = new Database();
            database.KeepConnectionAlive = true;
        }

        public object Get(string key)
        {
            #region 从数据库获取数据

            var table = database.ExecuteQuery(
                "select * from cacheinfo where _key=@key limit 1",
                new SQLiteParameter("@key", key));

            #endregion

            #region 没有数据 返回 null

            if (table.Rows.Count == 0)
            {
                return null;
            }

            #endregion

            #region 数据已过期 返回 null

            var slidingExpiration = table.Rows[0]["_slidingExpiration"].ToString().AsTimeSpan();
            if (slidingExpiration.Ticks > 0)
            {
                var lastTime = table.Rows[0]["_lastTime"].ToString().AsDateTime();
                if (lastTime.Add(slidingExpiration) < DateTime.Now)
                {
                    this.Remove(key);
                    return null;
                }

                #region 更新最后反问时间

                database.ExecuteNonQuery(
                   "update cacheinfo set _lastTime=@lastTime where _key=@key",
                   new SQLiteParameter("@key", key),
                   new SQLiteParameter("@lastTime", DateTime.Now.ToString())
                );

                #endregion
            }

            #endregion

            return table.Rows[0]["_value"].ToString();
        }

        void ICache.Set(string key, CacheInfo cache)
        {
            var value = this.Get(key);
            if (value != null)
            {
                this.Remove(key);
            }
            database.ExecuteScalar(
               @"insert into cacheinfo (_key, _value, _created, _lastUpdateUsage, _slidingExpiration, _absoluteExpiration) 
                                         values (@Key, @Value, @Created, @LastUpdateUsage, @SlidingExpiration, @AbsoluteExpiration)",
               new SQLiteParameter("@Key", key),
               new SQLiteParameter("@Value", cache.Value.ToJson()),
               new SQLiteParameter("@Created", DateTime.Now),
               new SQLiteParameter("@LastUpdateUsage", DateTime.Now),
               new SQLiteParameter("@SlidingExpiration", cache.SlidingExpiration),
               new SQLiteParameter("@AbsoluteExpiration", cache.SlidingExpiration)
            );
        }

        public void Remove(string key)
        {
            var value = database.ExecuteNonQuery(
                "delete from cacheinfo where _key=@key",
                new SQLiteParameter("@key", key)
            );
        }

        public IEnumerable<string> GetAllKeys()
        {
            var tb = database.ExecuteQuery("select * from cacheinfo");
            foreach (DataRow item in tb.Rows)
            {
                yield return item["Key"].ToString();
            }
        }

    }
}
