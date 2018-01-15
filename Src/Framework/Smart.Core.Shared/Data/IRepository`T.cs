using System.Collections.Generic;

namespace Smart.Core.Data
{
    /// <summary>
    /// Repository 接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        T Get(string predicate, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IEnumerable<T> Query(string sql, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
