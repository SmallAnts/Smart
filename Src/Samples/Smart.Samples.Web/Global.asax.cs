using Smart.Core;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Smart.Web.Mvc.Extensions;
using System.Threading.Tasks;
using Common.Logging;
using Smart.Core.Extensions;

namespace Smart.Samples.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            new SmartContext().InitializeMvc();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                MiniProfilerEF6.Initialize();
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            var log = SmartContext.Current.Resolve<ILog>();
            log.Error(e.Exception.GetBaseException());
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
