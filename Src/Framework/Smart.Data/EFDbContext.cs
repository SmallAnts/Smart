using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace Smart.Data
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class EFDbContext : DbContext
    {
        #region 构造函数

        public EFDbContext() : base("name=DefaultConnection")
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnPreModelCreating(modelBuilder, Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        protected virtual void OnPreModelCreating(DbModelBuilder modelBuilder, Assembly assembly)
        {
            if (assembly == null) return;

            // 自动添加映射配置
            var typesToRegister = assembly.GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

        }
    }
}