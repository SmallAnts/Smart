using Autofac;
using Smart.Core.Configuration;
using Smart.Core.Data;
using Smart.Core.Dependency;
using Smart.Data;
using System.Data.Entity;

namespace Smart.Samples.Domain
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order { get { return 0; } }      

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            builder.RegisterType<Context.SampleDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<Context.OtherDbContext>().As<Context.IOtherDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EFRepository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
        }
    }
}
