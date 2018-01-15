using System;
using Autofac;
using System.Web;
using Smart.Core.Extensions;

namespace Smart.Core.Dependency
{
    /// <summary>
    /// 服务和依赖关系解析接口
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// 获取依赖注入服务
        /// </summary>
        /// <param name="serviceType">注册的服务类型</param>
        /// <param name="serviceName">注册的服务名</param>
        /// <returns></returns>
        object Resolve(Type serviceType, string serviceName = null);

        /// <summary>
        /// 获取依赖注入服务
        /// </summary>
        /// <param name="serviceName">注册的服务名</param>
        /// <returns></returns>
        T Resolve<T>(string serviceName = null);
    }

    /// <summary>
    /// 支持HTTP的依赖解析器 
    /// </summary>
    public class HttpDependencyResover : IDependencyResolver
    {
        /// <summary>
        /// 
        /// </summary>
        protected ILifetimeScope Scope
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var scope = HttpContext.Current.Items["smart_autofac_scope"] as ILifetimeScope;
                    if (scope == null)
                    {
                        scope = ((ContainerManager)SmartContext.Current).BeginLifetimeScope();
                        HttpContext.Current.Items["smart_autofac_scope"] = scope;
                    }
                    return scope;
                }
                return ((ContainerManager)SmartContext.Current)._container;
            }
        }

        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="serviceName">服务注册名称</param>
        /// <returns></returns>
        public object Resolve(Type serviceType, string serviceName = null)
        {
            if (serviceName.IsEmpty())
            {
                return Scope.Resolve(serviceType);
            }
            else
            {
                return Scope.ResolveNamed(serviceName, serviceType);
            }
        }
        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="serviceName">服务注册名称</param>
        /// <returns></returns>
        public T Resolve<T>(string serviceName = null)
        {
            if (serviceName.IsEmpty())
            {
                return Scope.Resolve<T>();
            }
            else
            {
                return Scope.ResolveNamed<T>(serviceName);
            }
        }
    }
}
