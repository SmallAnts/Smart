using System.Data.Entity;
using System.Reflection;
using Smart.Data.Extensions;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.Diagnostics;

namespace Smart.Sample.Core.Context
{
    internal partial class SampleDbContext : Smart.Data.EF.EFDbContext
    {
        static SampleDbContext()
        {
#if DEBUG
            Database.SetInitializer<SampleDbContext>(null);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CashierDbContext>());
#else
           Database.SetInitializer<SampleDbContext>(null);
#endif
        }
        public SampleDbContext(bool initialize = false) : base("name=DefaultConnection")
        {
#if DEBUG
            Database.Log = log => Debug.WriteLine(log);
#endif
            Database.Initialize(initialize);
            //this.Configuration.AutoDetectChangesEnabled = false;//关闭自动跟踪对象的属性变化
            this.Configuration.LazyLoadingEnabled = false; //关闭延迟加载
            this.Configuration.ProxyCreationEnabled = false; //关闭代理类
            //this.Configuration.ValidateOnSaveEnabled = false; //关闭保存时的实体验证
            this.Configuration.UseDatabaseNullSemantics = true; //关闭数据库null比较行为
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.BuilderPrimaryKey(modelBuilder);
            //smodelBuilder.HasDefaultSchema("");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // 移除复数表名约定
            modelBuilder.AddMappings(Assembly.GetExecutingAssembly());
        }

    }
}
