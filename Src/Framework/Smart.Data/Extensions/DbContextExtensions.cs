using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.Common;
using System.Reflection;
using Smart.Core.Extensions;
using System.Data;

namespace Smart.Data.Extensions
{
    /// <summary>
    /// DbContext 扩展方法
    /// </summary>
    public static class DbContextExtensions
    {
        private static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxSelect = new Regex(@"\A\s*(SELECT|EXECUTE|CALL|WITH)\s", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static Regex rxParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        /// <summary>
        /// 创建数据库脚本
        /// </summary>
        /// <returns>生成数据库的SQL</returns>
        public static string CreateDatabaseScript(this DbContext db)
        {
            return ((IObjectContextAdapter)db).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="parameterName">参数名称，不用加前缀</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="parameterDirection">参数输入输出类型</param>
        /// <returns></returns>
        public static DbParameter CreateDbParameter(this DbContext db, string parameterName, DbType dbType, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            var factory = db.GetDbProviderFactory();
            var parameter = factory.CreateParameter();
            parameter.ParameterName = db.GetParameterPrefix() + parameterName;
            parameter.DbType = dbType;
            parameter.Direction = parameterDirection;
            return parameter;
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity">实体对象</param>
        public static void Detach(this DbContext db, object entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            ((IObjectContextAdapter)db).ObjectContext.Detach(entity);
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        public static void Detach<T>(this DbContext db, T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException("entity");
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            oc.Detach(entity);
        }

        /// <summary>
        /// 分离已经存在实体，根据传入实体的主键信息匹配
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库上下文对象</param>
        /// <param name="obj">要分离的实体</param>
        /// 
        public static void DetachOther<T>(this DbContext db, T obj) where T : class
        {
            var local = db.FindLocal(obj);
            if (local != null)
            {
                db.Detach(local);
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响行数。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql">要执行的Sql语句,如:update userinfo set name=@0 where id=@1</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int ExecuteSql(this DbContext db, string sql, params object[] parameters)
        {
            //sql.ThrowIfNull("sql");
            using (var cmd = db.CreateDbCommand())
            {
                var ps = db.ProcessParams(sql, parameters);
                cmd.CommandText = ps.Item1;
                cmd.Parameters.AddRange(ps.Item2.ToArray());
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响行数。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqls">要执行的Sql语句集合</param>
        /// <param name="parameters">参数列表集合</param>
        /// <returns></returns>
        public static int ExecuteSql(this DbContext db, List<string> sqls, List<object[]> parameters)
        {
            sqls.ThrowIfNull("sqls");
            parameters.ThrowIfInvalidOperation(() => parameters != null && sqls.Count != parameters.Count, "sqls 和 parameters 的个数必须一致！");
            using (var cmd = db.CreateDbCommand())
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var reslut = 0;
                for (int i = 0; i < sqls.Count; i++)
                {
                    var ps = db.ProcessParams(sqls[i], parameters == null ? null : parameters[i]);
                    cmd.CommandText = ps.Item1;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(ps.Item2.ToArray());
                    reslut += cmd.ExecuteNonQuery();
                }
                return reslut;
            }
        }

        /// <summary>
        /// 执行SQL语句，并返第一行第一列的值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql">要执行的Sql语句,如:select count(1) from sysuser</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(this DbContext db, string sql, params object[] parameters)
        {
            using (var cmd = db.CreateDbCommand())
            {
                var ps = db.ProcessParams(sql, parameters);
                cmd.CommandText = ps.Item1;
                cmd.Parameters.AddRange(ps.Item2.ToArray());
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行存储过程。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="paramObject">输入参数对象，一般使用一个匿名类型，也可以为null</param>
        /// <param name="parameters">参数数组，DbParameter 使用 db.</param>
        /// <returns>返回受影响行数</returns>
        public static int ExecuteProc(this DbContext db, string procName, object paramObject, params DbParameter[] parameters)
        {
            using (var cmd = db.CreateDbCommand())
            {
                cmd.CommandText = procName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region 参数处理

                if (paramObject != null)
                {
                    var inParams = db.CreateParameters(paramObject);
                    cmd.Parameters.AddRange(inParams.ToArray());
                }

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                #endregion
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                var result = cmd.ExecuteNonQuery();
                return result;
            }
        }
        /// <summary>
        /// 执行存储过程。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="paramObject">输入参数对象，一般使用一个匿名类型，也可以为null</param>
        /// <param name="parameters">参数数组，DbParameter 使用 db.</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteProcDataSet(this DbContext db, string procName, object paramObject, params DbParameter[] parameters)
        {
            using (var cmd = db.CreateDbCommand())
            {
                cmd.CommandText = procName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region 参数处理

                if (paramObject != null)
                {
                    var inParams = db.CreateParameters(paramObject);
                    cmd.Parameters.AddRange(inParams.ToArray());
                }

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                #endregion
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var ds = new DataSet();
                var ada = db.GetDbProviderFactory().CreateDataAdapter();
                ada.SelectCommand = cmd;
                ada.Fill(ds);
                return ds;
            }
        }

        /// <summary>
        /// 获取数据库抽象工厂类
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(this DbContext db)
        {
            var conn = db.Database.Connection;
            var pi = conn.GetType().GetProperty("DbProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            return pi.GetValue(conn, null) as DbProviderFactory;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetDbType(this DbContext db)
        {
            string dbtype = db.Database.Connection.GetType().Name;
            if (dbtype.Contains("MySql")) return "MYSQL";
            else if (dbtype.Contains("SqlCe")) return "SQLSERVERCE";
            else if (dbtype.Contains("Npgsql")) return "POSTGRESQL";
            else if (dbtype.Contains("Oracle")) return "ORACLE";
            else if (dbtype.Contains("SQLite")) return "SQLITE";
            else if (dbtype.Contains("Sql")) return "SQLSERVER";
            else return dbtype.Replace("Connection", "");
        }

        /// <summary>
        /// 获取数据库参数前缀字符
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetParameterPrefix(this DbContext db)
        {
            var dbType = db.GetDbType();
            switch (dbType)
            {
                case "MYSQL":
                    return "?";
                case "ORACLE":
                    return ":";
                default:
                    return "@";
            }
        }

        /// <summary>
        /// 获取实体的主键信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetEntityKeys<T>(this DbContext db) where T : class
        {
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            var keys = oc.CreateObjectSet<T>().EntitySet.ElementType.KeyProperties.Select(x => x.Name);
            return keys;
        }

        /// <summary>
        /// 在Local中查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindLocal<T>(this DbContext db, T obj) where T : class
        {
            var keys = db.GetEntityKeys<T>();
            var func = GetFindExp<T>(obj, keys).Compile();
            return db.Set<T>().Local.FirstOrDefault(func);
        }
        private static Expression<Func<T, bool>> GetFindExp<T>(T obj, IEnumerable<string> keys) where T : class
        {
            var p = Expression.Parameter(typeof(T), "x");

            var keyexps = keys.Select(x =>
            {
                var member = Expression.PropertyOrField(p, x);
                var objV = typeof(T).GetProperty(x).GetValue(obj, null);
                var eq = Expression.Equal(member, Expression.Constant(objV));
                return eq;
            }).ToList();

            if (keys.Count() == 1)
            {
                return Expression.Lambda<Func<T, bool>>(keyexps[0], new[] { p });
            }

            var and = Expression.AndAlso(keyexps[0], keyexps[1]);
            for (var i = 2; i < keyexps.Count; i++)
            {
                and = Expression.AndAlso(and, keyexps[i]);
            }
            return Expression.Lambda<Func<T, bool>>(and, new[] { p });
        }

        /// <summary>
        /// 批量移除实体，执行 SaveChanges 后提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sql">要执行的Sql语句,如:select * from userinfo where id=@0</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this DbContext db, string sql, params object[] parameters)
        {
            if (!rxSelect.IsMatch(sql))
            {
                sql = $"select t.* from {typeof(T).Name} t where {sql}";
            }
            var ps = db.ProcessParams(sql, parameters);
            sql = ps.Item1;
            parameters = ps.Item2.ToArray();
            return db.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// 通过sql查询返回DataTable
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql">要执行的Sql语句,如:select * from userinfo where id=@0</param>
        /// <param name="parameters">参数</param>  
        /// <returns></returns>
        public static DataTable QueryDataTable(this DbContext db, string sql, params object[] parameters)
        {
            using (var cmd = db.CreateDbCommand())
            {
                var ps = db.ProcessParams(sql, parameters);
                cmd.CommandText = ps.Item1;
                cmd.Parameters.AddRange(ps.Item2.ToArray());
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(dataReader);
                    return dt;
                }
            }

        }

        /// <summary>
        ///  通过sql查询返回动态列表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql">要执行的Sql语句,如:select * from userinfo where id=@0</param>
        /// <param name="parameters">参数</param>  
        /// <returns></returns>
        public static IEnumerable<dynamic> QueryDynamic(this DbContext db, string sql, params object[] parameters)
        {
            using (var cmd = db.CreateDbCommand())
            {
                var ps = db.ProcessParams(sql, parameters);
                cmd.CommandText = ps.Item1;
                cmd.Parameters.AddRange(ps.Item2.ToArray());
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                //var retObject = new List<dynamic>();
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return dataReader.AsDynamic();
                    }
                }
            }
        }

        /// <summary>
        /// 执行分页查询,返回分页结果
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sqlText">完整的分页前的查询语句</param>
        /// <param name="parameters">查询SQL的参数列表</param>
        /// <returns>分页数据对象</returns>
        public static Page<T> QueryPage<T>(this DbContext db, int pageIndex, int pageSize, string sqlText, params object[] parameters) where T : class
        {
            if (!rxSelect.IsMatch(sqlText))
            {
                sqlText = $"select t.* from {typeof(T).Name} t where {sqlText}";
            }

            string sqlCount, sqlPage;
            // 生成分页查询
            db.BuildPageQueries((pageIndex - 1) * pageSize, pageSize, sqlText, ref parameters, out sqlCount, out sqlPage);
            // 初始化返回结果
            var sql = db.ProcessParams(sqlCount, parameters);
            var result = db.Database.SqlQuery<Page<T>>(sql.Item1, sql.Item2.ToArray()).First();
            result.CurrentPage = pageIndex;
            result.PageSize = pageSize;
            //result.TotalPages = result.TotalItems / pageSize;
            //if ((result.TotalItems % pageSize) != 0)
            //    result.TotalPages++;
            // 执行查询获取数据
            sql = db.ProcessParams(sqlPage, parameters);
            result.Items = db.Database.SqlQuery<T>(sql.Item1, sql.Item2.ToArray()).ToList();
            return result;
        }

        /// <summary>
        /// 移除实体, 执行 SaveChanges 后提交
        /// <para></para>
        /// <para>示例：</para>
        /// <para>dbContext.Remoe(model);</para>
        /// <para>dbContext.SaveChanges();</para>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="db"></param>
        /// <param name="entity">要删除的实体，实体中只需要主键字段信息</param>
        public static void Remove<TEntity>(this DbContext db, TEntity entity) where TEntity : class
        {
            db.DetachOther(entity);
            var entry = db.Entry(entity);
            db.Set<TEntity>().Attach(entity);
            entry.State = EntityState.Deleted;
        }

        /// <summary>
        /// 移除实体, 执行 SaveChanges 后提交
        /// <para></para>
        /// <para>示例：</para>
        /// <para>dbContext.Remoe(entites);</para>
        /// <para>dbContext.SaveChanges();</para>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="db"></param>
        /// <param name="entites">要删除的实体列表，实体中只需要主键字段信息</param>
        public static void Remove<TEntity>(this DbContext db, IEnumerable<TEntity> entites) where TEntity : class
        {
            foreach (var entity in entites)
            {
                db.DetachOther(entity);
                var entry = db.Entry(entity);
                db.Set<TEntity>().Attach(entity);
                entry.State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 批量更新实体, 执行 SaveChanges 后提交
        /// <para></para>
        /// <para>示例：</para>
        /// <para>更新所有字段：dbContext.Update(models);</para>
        /// <para>更新一个字段：dbContext.Update(models, updateProperties : m=>m.Name);</para>
        /// <para>更新多个字段：dbContext.Update(models, updateProperties : m=>new {m.Name, m.Phone});</para>
        /// <para>忽略多个字段：dbContext.Update(models, ignoreProperties  : m=>new {m.Name, m.Phone});</para>
        /// <para>提交所有变更：dbContext.SaveChanges();</para>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="db"></param>
        /// <param name="entites">要更新的实体</param>
        /// <param name="updateProperties">要更新的字段</param>
        /// <param name="ignoreProperties">要忽略更新的字段</param>
        public static void Update<TEntity>(this DbContext db, IEnumerable<TEntity> entites,
            Expression<Func<TEntity, object>> updateProperties = null,
            Expression<Func<TEntity, object>> ignoreProperties = null) where TEntity : class
        {
            foreach (var item in entites)
            {
                Update(db, item, updateProperties, ignoreProperties);
            }
        }

        /// <summary>
        /// 更新实体, 执行 SaveChanges 后提交
        /// <para></para>
        /// <para>示例：</para>
        /// <para>更新所有字段：dbContext.Update(model);</para>
        /// <para>更新一个字段：dbContext.Update(model, updateProperties : m=>m.Name);</para>
        /// <para>更新多个字段：dbContext.Update(model, updateProperties : m=>new {m.Name, m.Phone});</para>
        /// <para>忽略多个字段：dbContext.Update(model, ignoreProperties  : m=>new {m.Name, m.Phone});</para>
        /// <para>提交所有变更：dbContext.SaveChanges();</para>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="db"></param>
        /// <param name="entity">要更新的实体</param>
        /// <param name="updateProperties">要更新的字段</param>
        /// <param name="ignoreProperties">要忽略更新的字段</param>
        public static void Update<TEntity>(this DbContext db, TEntity entity,
            Expression<Func<TEntity, object>> updateProperties = null,
            Expression<Func<TEntity, object>> ignoreProperties = null) where TEntity : class
        {
            if (updateProperties != null && ignoreProperties != null)
                throw new ArgumentException("不能同时指定 更新字段和忽略字段参数");

            db.DetachOther(entity);
            db.Set<TEntity>().Attach(entity);
            var entry = db.Entry(entity);
            if (updateProperties != null)
            {
                // 指定更新部分字段
                var updateProps = GetProperties(entity, updateProperties);
                foreach (var item in updateProps)
                {
                    entry.Property(item).IsModified = true;
                }
            }
            else if (ignoreProperties != null)
            {
                // 忽略指定字段更新
                entry.State = EntityState.Modified;

                var ignoreProps = GetProperties(entity, ignoreProperties);
                foreach (var item in ignoreProps)
                {
                    entry.Property(item).IsModified = false;
                }
            }
            else
            {
                // 指定更新整个实体
                entry.State = EntityState.Modified;
            }
        }

        private static string[] GetProperties<TEntity>(TEntity entity, Expression<Func<TEntity, object>> selectorMember) where TEntity : class
        {
            if (selectorMember == null) return null;

            string[] properties = null;
            if (selectorMember.Body is MemberExpression)
            {
                properties = new string[] { (selectorMember.Body as MemberExpression).Member.Name };
            }
            else if (selectorMember.Body is UnaryExpression) // 可空类型成员
            {
                var me = (selectorMember.Body as UnaryExpression).Operand as MemberExpression;
                if (me != null) properties = new string[] { me.Member.Name };
            }
            else
            {
                var pis = selectorMember.Compile()(entity).GetType().GetProperties();
                properties = new string[pis.Length];
                for (int i = 0; i < pis.Length; i++)
                {
                    properties[i] = pis[i].Name;
                }
            }
            return properties;
        }

        /// <summary>
        /// 创建DbCommand实例
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static DbCommand CreateDbCommand(this DbContext db)
        {
            var cmd = db.Database.Connection.CreateCommand();
            #region 如果是ODP.NET提供程序，则设置为按名称绑定参数
            if (db.GetDbType() == "ORACLE")
            {
                var pi = cmd.GetType().GetProperty("BindByName");
                if (pi != null) pi.SetValue(cmd, true, null);
            }
            #endregion
            return cmd;
        }

        /// <summary>
        /// 根据对象创建参数列表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal static List<DbParameter> CreateParameters(this DbContext db, object parameters)
        {
            var dbProviderFactory = db.GetDbProviderFactory();
            var pis = parameters.GetType().GetProperties();
            List<DbParameter> paramList = new List<DbParameter>();
            foreach (var item in pis)
            {
                var param = dbProviderFactory.CreateParameter();
                param.ParameterName = db.GetParameterPrefix() + item.Name;
                param.Value = item.GetValue(parameters, null);
                paramList.Add(param);
            }
            return paramList;
        }

        /// <summary>
        /// 生成分页查询语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <param name="sqlCount"></param>
        /// <param name="sqlPage"></param>
        internal static void BuildPageQueries(this DbContext db, long skip, long take, string sql, ref object[] args, out string sqlCount, out string sqlPage)
        {
            var dbType = db.GetDbType();
            string sqlSelectRemoved, sqlOrderBy;
            if (!SplitSqlForPaging(sql, out sqlCount, out sqlSelectRemoved, out sqlOrderBy))
                throw new Exception("无法解析为分页查询的SQL语句");
            if (dbType == "ORACLE" && sqlSelectRemoved.StartsWith("*"))
                throw new Exception("执行分页查询时必须为‘*’指定别名。如： select t.* from table t order by t.id");

            // Build the SQL for the actual final result
            if (dbType == "SQLSERVER" || dbType == "ORACLE")
            {
                sqlSelectRemoved = DbContextExtensions.rxOrderBy.Replace(sqlSelectRemoved, "");
                if (rxDistinct.IsMatch(sqlSelectRemoved))
                {
                    sqlSelectRemoved = "peta_inner.* FROM (SELECT " + sqlSelectRemoved + ") peta_inner";
                }
                var orderby = sqlOrderBy.IsEmpty() ? ("ORDER BY (SELECT NULL" + (dbType == "ORACLE" ? " FROM DUAL" : "") + ")") : sqlOrderBy;
                sqlPage = $"SELECT * FROM (SELECT ROW_NUMBER() OVER ({orderby}) _rownum,{ sqlSelectRemoved}) _paged WHERE _rownum>@{ args.Length} AND _rownum<=@{args.Length + 1}";
                args = args.Concat(new object[] { skip, skip + take }).ToArray();
            }
            else if (dbType == "SQLSERVERCE")
            {
                sqlPage = $"{sql}\nOFFSET @{args.Length} ROWS FETCH NEXT @{ args.Length + 1} ROWS ONLY";
                args = args.Concat(new object[] { skip, take }).ToArray();
            }
            else
            {
                sqlPage = $"{sql}\nLIMIT @{args.Length} OFFSET @{args.Length + 1}";
                args = args.Concat(new object[] { take, skip }).ToArray();
            }
        }

        /// <summary>
        /// 将SQL转成分页查询SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlCount">总记录数统计SQL</param>
        /// <param name="sqlSelectRemoved"></param>
        /// <param name="sqlOrderBy">排序条件</param>
        /// <returns></returns>
        internal static bool SplitSqlForPaging(string sql, out string sqlCount, out string sqlSelectRemoved, out string sqlOrderBy)
        {
            sqlSelectRemoved = null;
            sqlCount = null;
            sqlOrderBy = null;

            var m = rxColumns.Match(sql);
            if (!m.Success)
                return false;

            Group g = m.Groups[1];
            sqlSelectRemoved = sql.Substring(g.Index);

            if (rxDistinct.IsMatch(sqlSelectRemoved))
                sqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") TotalItems " + sql.Substring(g.Index + g.Length);
            else
                sqlCount = sql.Substring(0, g.Index) + "COUNT(*) TotalItems " + sql.Substring(g.Index + g.Length);

            m = rxOrderBy.Match(sqlCount);
            if (!m.Success)
            {
                sqlOrderBy = null;
            }
            else
            {
                g = m.Groups[0];
                sqlOrderBy = g.ToString();
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }

            return true;
        }

        /// <summary>
        /// 参数处理
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static Tuple<string, List<object>> ProcessParams(this DbContext db, string sql, object[] args)
        {
            var prefix = db.GetParameterPrefix();
            var dbProviderFactory = db.GetDbProviderFactory();
            var parameters = new List<object>();
            var sqlText = rxParamsPrefix.Replace(sql, m =>
            {
                string paramName = m.Value.Substring(1);
                object paramValue;
                int paramIndex;

                if (int.TryParse(paramName, out paramIndex))
                {
                    #region 验证参数数值和个数是否匹配

                    if (paramIndex < 0 || paramIndex >= args.Length)
                        throw new ArgumentOutOfRangeException($"参数 '@{paramIndex}' 不在指定的范围内。");
                    paramValue = args[paramIndex];
                    #endregion
                }
                else
                {
                    #region 验证参数名称是不是和对象属性名称一致

                    bool found = false;
                    paramValue = null;
                    foreach (var arg in args)
                    {
                        var argType = arg.GetType();
                        if (argType.IsValueType || argType == typeof(string))
                        {
                            if (paramName == nameof(arg))
                            {
                                paramValue = arg;
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                            var pi = argType.GetProperty(paramName);
                            if (pi != null)
                            {
                                paramValue = pi.GetValue(arg, null);
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                        throw new ArgumentException($"无法获取参数{paramName}的属性值");
                    #endregion
                }

                #region 创建参数

                // 可迭代类型，但不是 string 和 byte[]
                if ((paramValue as System.Collections.IEnumerable) != null
                     && (paramValue as string) == null
                     && (paramValue as byte[]) == null)
                {
                    var sb = new StringBuilder();
                    foreach (var value in paramValue as System.Collections.IEnumerable)
                    {
                        sb.Append((sb.Length == 0 ? prefix : "," + prefix));
                        sb.Append(parameters.Count.ToString());
                        var param = dbProviderFactory.CreateParameter();
                        param.ParameterName = paramName;
                        param.Value = value;
                        parameters.Add(param);
                    }
                    return sb.ToString();
                }
                else
                {
                    var param = dbProviderFactory.CreateParameter();
                    param.ParameterName = parameters.Count.ToString();
                    param.Value = paramValue;
                    parameters.Add(param);
                    return prefix + param.ParameterName;
                }
                #endregion

            });

            var result = new Tuple<string, List<object>>(sqlText, parameters);
            return result;
        }
    }
}
