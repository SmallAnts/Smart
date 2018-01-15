using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Smart.Data.EF
{
    public interface IEFRepository<T> : Core.Data.IRepository<T>
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        T Get(Expression<Func<T, bool>> predicate);
        bool Exists(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count);
    }
    public interface IEFRepository<TEntity, TDbContent> : Core.Data.IRepository<TEntity, TDbContent>
    {
    }
}
