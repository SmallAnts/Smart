using Smart.Core.Extensions;
using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

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

        public T Get<T>(string key) where T : class
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

            return table.Rows[0]["_value"].ToString().JsonTo<T>();
        }

        public void Remove(string key)
        {
            var value = database.ExecuteNonQuery(
                "delete from cacheinfo where _key=@key",
                new SQLiteParameter("@key", DbType.String, key)
            );
        }

        public void RemoveAll(Predicate<string> match)
        {
            if (match == null) return;
            //database.KeepConnectionAlive = true;
            var tb = database.ExecuteQuery("select * from cacheinfo");
            foreach (DataRow item in tb.Rows)
            {
                if (match(item["Key"].ToString()))
                {
                    this.Remove(item["Key"].ToString());
                }
            }
            //database.KeepConnectionAlive = false;
            //database.CloseConnection();
        }

        public T Set<T>(string key, CacheInfo<T> cache) where T : class
        {
            var value = this.Get<T>(key);
            if (value != null)
            {
                this.Remove(key);
            }
            database.ExecuteScalar(
               "insert into cacheinfo (_key,_value,_slidingExpiration,_lastTime) values (@Key,@Value,@SlidingExpiration,@LastTime)",
               new SQLiteParameter("@Key", key),
               new SQLiteParameter("@Value", cache.Value.ToJson()),
               new SQLiteParameter("@SlidingExpiration", cache.SlidingExpiration.ToString()),
               new SQLiteParameter("@LastTime", DateTime.Now.ToString())
            );
            return value;
        }
    }
}
