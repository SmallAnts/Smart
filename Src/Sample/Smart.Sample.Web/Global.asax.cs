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
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using FluentValidation.Mvc;
using FluentValidation.Attributes;

namespace YQ.Cashier.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            new SmartContext().InitializeMvc();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                MiniProfilerEF6.Initialize();
            }
            else {
                // 预热EntityFramework
                using (var dbcontext = SmartContext.Current.Resolve<DbContext>())
                {
                    var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                    var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                    mappingCollection.GenerateViews(new List<EdmSchemaError>());
                }
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var log = SmartContext.Current.Resolve<ILog>();
            log.Error((e.ExceptionObject as Exception).GetBaseException());
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
