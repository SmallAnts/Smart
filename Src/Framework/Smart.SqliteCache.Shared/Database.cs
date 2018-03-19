using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Smart.Core.Caching
{
    internal class Database
    {
        string _connectionString;
        int _sharedConnectionDepth = 0;
        SQLiteConnection _sharedConnection;
        public SQLiteConnection Connection
        {
            get { return _sharedConnection; }
        }
        public bool KeepConnectionAlive { get; set; }
        /// <summary>
        /// 构造函数，如果没有数据则创建数据库
        /// </summary>
        public Database()
        {
            string cacheDbName = "_cache.db";
            _connectionString = $"Data Source={cacheDbName};Version=3;";
            if (!File.Exists(cacheDbName))
            {
                //创建一个数据库文件。
                SQLiteConnection.CreateFile(cacheDbName);

                string createTb = @"
CREATE TABLE cacheinfo(
   _key TEXT PRIMARY KEY NOT NULL,
   _value TEXT NOT NULL,
   _lastTime TEXT NOT NULL,
   _slidingExpiration TEXT NOT NULL
);";
                ExecuteNonQuery(createTb);
            }
        }

        public void OpenConnection()
        {
            if (_sharedConnectionDepth == 0)
            {
                _sharedConnection = new SQLiteConnection(this._connectionString);
                _sharedConnection.ConnectionString = _connectionString;

                if (_sharedConnection.State == ConnectionState.Broken)
                    _sharedConnection.Close();

                if (_sharedConnection.State == ConnectionState.Closed)
                    _sharedConnection.Open();

                if (KeepConnectionAlive)
                    _sharedConnectionDepth++;
            }
            _sharedConnectionDepth++;
        }

        /// <summary>
        ///     Releases the shared connection
        /// </summary>
        public void CloseConnection()
        {
            if (_sharedConnectionDepth > 0)
            {
                _sharedConnectionDepth--;
                if (_sharedConnectionDepth == 0)
                {
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }
            }
        }

        /// <summary>   
        /// 对 SQLite 数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句。</param>   
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            this.OpenConnection();
            using (var command = new SQLiteCommand(this.Connection))
            {
                command.CommandText = sql;
                if (parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                affectedRows = command.ExecuteNonQuery();
            }
            this.CloseConnection();
            return affectedRows;
        }

        /// <summary>  
        /// 执行查询语句，并返回第一个结果。  
        /// </summary>  
        /// <param name="sql">查询语句。</param>  
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns>查询结果。</returns>  
        /// <exception cref="Exception"></exception>  
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            object result;
            this.OpenConnection();
            using (var cmd = new SQLiteCommand(this.Connection))
            {
                cmd.CommandText = sql;
                if (parameters.Length != 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                result = cmd.ExecuteScalar();
            }
            this.CloseConnection();
            return result;
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。   
        /// </summary>   
        /// <param name="sql">要执行的查询语句。</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            var data = new DataTable();
            this.OpenConnection();
            using (var command = new SQLiteCommand(sql, this.Connection))
            {
                if (parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                using (var adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(data);
                }
            }
            this.CloseConnection();
            return data;
        }
    }
}
