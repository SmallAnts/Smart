using Autofac;
using Autofac.Integration.Mvc;
using Smart.Core.Configuration;
using Smart.Core.Dependency;
using System.Linq;

namespace Smart.Web.Mvc
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            //Controllers
            builder.RegisterFilterProvider();
            builder.RegisterControllers(config.TypeFinder.Assemblies.ToArray());
        }
    }
}
