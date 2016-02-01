using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Smart.Core.Data
{
    /// <summary>
    /// Repository 接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T> where T : class, IEntity
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        T Get(object id);
        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        bool Exists(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order);
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count);
    }

    /// <summary>
    /// Repository 接口，支持多数据库
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContext">用来标识不同数据库的空接口</typeparam>
    public interface IRepository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
