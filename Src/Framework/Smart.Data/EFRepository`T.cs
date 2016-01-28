using Smart.Core.Data;
using Smart.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Smart.Data
{
    /// <summary>
    /// 基于 EntityFramework 的 Repository
    /// </summary>
    public class EFRepository<T> : IRepository<T> where T : class, IEntity
    {
        public EFRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region 私有成员

        protected DbContext _dbContext;
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

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public T Get(object id)
        {
            return this.Entities.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.FirstOrDefault(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Count(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Any(predicate);
        }

        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Where(predicate);
        }

        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable.Skip(skip).Take(count);
        }

        #region 显示实现接口
        IQueryable<T> IRepository<T>.Table
        {
            get
            {
                return Table;
            }
        }

        IQueryable<T> IRepository<T>.TableNoTracking
        {
            get
            {
                return TableNoTracking;
            }
        }

        void IRepository<T>.Insert(T entity)
        {
            Insert(entity);
        }

        void IRepository<T>.Update(T entity)
        {
            Update(entity);
        }

        void IRepository<T>.Delete(T entity)
        {
            Delete(entity);
        }

        T IRepository<T>.Get(object id)
        {
            return Get(id);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate);
        }

        bool IRepository<T>.Exists(Expression<Func<T, bool>> predicate)
        {
            return Exists(predicate);
        }

        int IRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate);
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate);
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            return Fetch(predicate, order);
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            return Fetch(predicate, order, skip, count);
        }
        #endregion

        #endregion
    }

    public class EFRepository<TEntity, TDbCoutext> : EFRepository<TEntity>, IRepository<TEntity, TDbCoutext> where TEntity : class, IEntity
    {
        public EFRepository(TDbCoutext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}