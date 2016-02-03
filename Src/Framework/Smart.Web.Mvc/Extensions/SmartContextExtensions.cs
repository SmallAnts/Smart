using Autofac.Integration.Mvc;
using Smart.Core.Configuration;
using Smart.Core.Dependency;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Smart.Web.Mvc.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SmartContextExtensions
    {
        /// <summary>
        /// 初始化一个静态实例工厂。
        /// </summary>
        /// <param name="forceRecreate">创建一个新工厂实例，即使工厂已被初始化。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainerManager InitializeMvc(this Core.SmartContext context, SmartConfig config = null, bool forceRecreate = false)
        {
            // 删除不必要的HTTP响应头 "X-AspNetMvc-Version"
            MvcHandler.DisableMvcResponseHeader = true;

            var icm = Core.SmartContext.Initialize(config, forceRecreate);
            var cm = icm as ContainerManager;
            if (cm != null)
            {
                DependencyResolver.SetResolver(new AutofacDependencyResolver(cm._container));
            }

            return cm;
        }
    }
}
