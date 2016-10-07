using Autofac;
using Smart.Core.Configuration;
using Smart.Core.Dependency;
using System.Data.Entity;

namespace Smart.Sample.Core
{
    // 依赖注册
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            builder.RegisterType<Context.SampleDbContext>().As<DbContext>().As<Context.SampleDbContext>().InstancePerDependency();
            //builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IEFRepository<>)).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(EFRepository<,>)).As(typeof(IEFRepository<,>)).InstancePerLifetimeScope();
        }
    }
}
