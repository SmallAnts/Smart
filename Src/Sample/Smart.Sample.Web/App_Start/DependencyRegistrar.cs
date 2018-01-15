using System;
using Autofac;
using Smart.Core.Configuration;
using Smart.Core.Dependency;
using System.Web.Mvc;

namespace Smart.Sample.Web
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            log4net.Config.XmlConfigurator.Configure();
            var log = log4net.LogManager.GetLogger(typeof(DependencyRegistrar));
            builder.RegisterInstance(log).As<log4net.ILog>();
  
        }
    }

  
}
