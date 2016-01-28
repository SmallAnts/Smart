using Autofac.Integration.Mvc;
using Smart.Core;
using Smart.Core.Configuration;
using Smart.Core.Infrastructure;
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
        }
    }
}
