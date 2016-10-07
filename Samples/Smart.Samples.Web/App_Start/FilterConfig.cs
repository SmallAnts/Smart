using Smart.Core;
using StackExchange.Profiling.Mvc;
using System.Web.Mvc;

namespace Smart.Samples.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (SmartContext.Config.DisplayMiniProfiler == true)
            {
                GlobalFilters.Filters.Add(new ProfilingActionFilter());
            }
            //filters.Add(new ErrorFilterAttribute());
            //filters.Add(new FormAuthorizeAttribute());
        }
    }
}