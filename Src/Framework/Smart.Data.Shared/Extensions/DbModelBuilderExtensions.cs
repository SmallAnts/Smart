using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;

namespace Smart.Data.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbModelBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ConfigurationRegistrar AddMappings(this DbModelBuilder modelBuilder, Assembly assembly)
        {
            if (assembly == null) return modelBuilder.Configurations;

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

            return modelBuilder.Configurations;
        }
    }
}
