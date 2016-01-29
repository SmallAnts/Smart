using Autofac.Integration.Mvc;
using Smart.Core;
using Smart.Core.Configuration;
using Smart.Core.Infrastructure;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StackExchange.Profiling.EntityFramework6;
using StackExchange.Profiling.Mvc;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Smart.Samples.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SmartContext.Initialize(new SmartConfig()
            {
                //TypeFinder = new DirectoryTypeFinder(null, "Smart*.dll"),
                OnDependencyRegistered = container =>
                {
                    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
                }
            });

            GlobalFilters.Filters.Add(new ProfilingActionFilter());
            MiniProfilerEF6.Initialize();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            MiniProfiler.Start();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            MiniProfiler.Stop();
        }
    }
}
