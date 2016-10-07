using Autofac;
using Smart.Core.Configuration;
using Smart.Core.Dependency;
using System.Reflection;

namespace Smart.Sample.Services
{
    // 依赖注入，服务类约定为以Service结尾的类
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            builder.RegisterType<Smart.Core.Caching.HttpCache>()
                .Named<Smart.Core.Caching.ICache>("ServiceCache")
                .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}
