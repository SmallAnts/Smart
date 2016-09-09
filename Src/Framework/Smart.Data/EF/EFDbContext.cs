using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Smart.Data.EF
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class EFDbContext : DbContext
    {
        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public EFDbContext() : this("name=DefaultConnection")
        {
            ////this.Configuration.AutoDetectChangesEnabled = false;//关闭自动跟踪对象的属性变化
            //this.Configuration.LazyLoadingEnabled = false; //关闭延迟加载
            //this.Configuration.ProxyCreationEnabled = false; //关闭代理类
            ////this.Configuration.ValidateOnSaveEnabled = false; //关闭保存时的实体验证
            //this.Configuration.UseDatabaseNullSemantics = true; //关闭数据库null比较行为
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConnectionString">示例：name=DefaultConnection</param>
        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException cex)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}