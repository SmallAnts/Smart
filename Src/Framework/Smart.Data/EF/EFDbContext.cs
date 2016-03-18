using System.Data.Entity;

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

        #endregion
    }
}