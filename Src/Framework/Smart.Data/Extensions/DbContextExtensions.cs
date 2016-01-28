using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Smart.Data.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// 创建数据库脚本
        /// </summary>
        /// <returns>生成数据库的SQL</returns>
        public static string CreateDatabaseScript(this DbContext db)
        {
            return ((IObjectContextAdapter)db).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public static void Detach(this DbContext db, object entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            ((IObjectContextAdapter)db).ObjectContext.Detach(entity);
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
        /// <param name="entity">要更新的实体</param>
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

            var entry = db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                db.Set<TEntity>().Attach(entity);
            }
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
        /// 移除实体, 执行 SaveChanges 后提交
        /// <para></para>
        /// <para>示例：</para>
        /// <para>dbContext.Remoe(model);</para>
        /// <para>dbContext.SaveChanges();</para>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="db"></param>
        /// <param name="entity">要删除的实体</param>
        public static void Remove<TEntity>(this DbContext db, TEntity entity) where TEntity : class
        {
            var entry = db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                db.Set<TEntity>().Attach(entity);
            }
            entry.State = EntityState.Deleted;
        }
    }
}
