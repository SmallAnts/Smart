using System;
using Autofac;

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
        /// <returns></returns>
        object GetService(Type serviceType);
    }
}
