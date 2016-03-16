using Smart.Core.Data;
using Smart.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Smart.Data.EF
{
    /// <summary>
    /// 基于 EntityFramework 的 Repository
    /// </summary>
    public class EFRepository<T> : IEFRepository<T> where T : class
    {
        static Regex rxSelect = new Regex(@"\A\s*(SELECT|EXECUTE|CALL|WITH)\s",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex rxParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        public EFRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
            var conn = dbContext.Database.Connection;
            var pi = conn.GetType().GetProperty("DbProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            _dbProviderFactory = pi.GetValue(conn, null) as DbProviderFactory;
        }

        #region 私有成员

        protected DbContext _dbContext;
        protected DbProviderFactory _dbProviderFactory;
        private DbSet<T> _entities;
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<T>();
                return _entities;
            }
        }

        #endregion

        #region  IRepository<T> 接口成员

        public int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public int Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public int Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public T Get(string predicate, params object[] args)
        {
            var sql = string.Format("select t.* from {0} t where {1}", typeof(T).Name, predicate);
            var ps = ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.SqlQuery<T>(sql, args).FirstOrDefault();
        }

        public IEnumerable<T> Query(string sql, params object[] args)
        {
            if (!rxSelect.IsMatch(sql))
            {
                sql = string.Format("select t.* from {0} t where {1}", typeof(T).Name, sql);
            }
            var ps = ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.SqlQuery<T>(sql, args);
        }

        public int Execute(string sql, params object[] args)
        {
            var ps = ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.ExecuteSqlCommand(sql, args);
        }

        #endregion

        #region  IEFRepository<T> 接口成员

        public IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.FirstOrDefault(predicate);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Where(predicate);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Query(predicate).AsQueryable<T>());
            order(orderable);
            return orderable.Queryable;
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            var orderable = new Orderable<T>(Query(predicate).AsQueryable<T>());
            order(orderable);
            return orderable.Queryable.Skip(skip).Take(count);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Count(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Any(predicate);
        }

        #endregion

        Tuple<string, List<object>> ProcessParams(string sql, object[] args)
        {
            var parameters = new List<object>();
            var sqlText = rxParamsPrefix.Replace(sql, m =>
            {
                string paramName = m.Value.Substring(1);
                object paramValue;
                int paramIndex;

                if (int.TryParse(paramName, out paramIndex))
                {
                    // 验证参数数值和个数是否匹配
                    if (paramIndex < 0 || paramIndex >= args.Length)
                        throw new ArgumentOutOfRangeException(string.Format("参数 '@{0}' 不在指定的范围内。", paramIndex));
                    paramValue = args[paramIndex];
                }
                else
                {
                    // 验证参数名称是不是和对象属性名称一致
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
                        else {
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
                        throw new ArgumentException(string.Format("无法获取参数{0}的属性值", paramName));
                }

                // 可迭代类型，但不是 string 和 byte[]
                if ((paramValue as System.Collections.IEnumerable) != null &&
                    (paramValue as string) == null &&
                    (paramValue as byte[]) == null)
                {
                    var sb = new StringBuilder();
                    foreach (var value in paramValue as System.Collections.IEnumerable)
                    {
                        sb.Append((sb.Length == 0 ? "@" : ",@") + parameters.Count.ToString());
                        var param = _dbProviderFactory.CreateParameter();
                        param.ParameterName = paramName;
                        param.Value = value;
                        parameters.Add(param);
                    }
                    return sb.ToString();
                }
                else
                {
                    var param = _dbProviderFactory.CreateParameter();
                    param.ParameterName = paramName;
                    param.Value = paramValue;
                    parameters.Add(param);
                    return "@" + (parameters.Count - 1).ToString();
                }

            });

            var result = new Tuple<string, List<object>>(sqlText, parameters);
            return result;
        }
    }

    public class EFRepository<TEntity, TDbCoutext> : EFRepository<TEntity>, IEFRepository<TEntity, TDbCoutext> where TEntity : class
    {
        public EFRepository(TDbCoutext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}