using Smart.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Smart.Core.Extensions;

namespace Smart.Data.EF
{
    /// <summary>
    /// 基于 EntityFramework 的 Repository
    /// </summary>
    public class EFRepository<T> : IEFRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public EFRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region 私有成员
        /// <summary>
        /// 
        /// </summary>
        protected DbContext _dbContext;
        private DbSet<T> _entities;
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _dbContext.DetachOther(entity);
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
                this._dbContext.Remove(entity);
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
                this._dbContext.Update(entity);
                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }
        public int Execute(string sql, params object[] args)
        {
            var ps = this._dbContext.ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.ExecuteSqlCommand(sql, args);
        }
        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public T Get(string predicate, params object[] args)
        {
            var sql = $"select t.* from {typeof(T).Name} t where {predicate}";
            var ps = this._dbContext.ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.SqlQuery<T>(sql, args).FirstOrDefault();
        }

        public IEnumerable<T> Query(string sql, params object[] args)
        {
            return this._dbContext.Query<T>(sql, args);
        }

        /// <summary>
        /// 执行分页查询,返回分页结果
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sqlText">完整的分页前的查询语句</param>
        /// <param name="parameters">查询SQL的参数列表</param>
        /// <returns>分页数据对象</returns>
        public virtual Page<T> GetPage(int pageIndex, int pageSize, string sqlText, params object[] parameters)
        {
            return this._dbContext.QueryPage<T>(pageIndex, pageSize, sqlText, parameters);
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

    }

    public class EFRepository<TEntity, TDbCoutext> : EFRepository<TEntity>, IEFRepository<TEntity, TDbCoutext> where TEntity : class
    {
        public EFRepository(TDbCoutext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}