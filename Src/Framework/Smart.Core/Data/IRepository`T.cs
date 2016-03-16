using System.Collections.Generic;

namespace Smart.Core.Data
{
    /// <summary>
    /// Repository 接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T>
    {
        int Insert(T entity);

        int Delete(T entity);

        int Update(T entity);

        T GetById(object id);

        T Get(string predicate, params object[] args);

        IEnumerable<T> Query(string sql, params object[] args);

        int Execute(string sql, params object[] args);

    }

    /// <summary>
    /// Repository 接口，支持多数据库
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContext">用来标识不同数据库的空接口</typeparam>
    public interface IRepository<TEntity, TDbContext> : IRepository<TEntity>
    {
    }
}
