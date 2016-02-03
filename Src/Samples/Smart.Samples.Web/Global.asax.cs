using Smart.Core;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Smart.Web.Mvc.Extensions;

namespace Smart.Samples.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            new SmartContext().InitializeMvc();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                MiniProfilerEF6.Initialize();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                MiniProfiler.Stop();
            }
        }
    }
}
