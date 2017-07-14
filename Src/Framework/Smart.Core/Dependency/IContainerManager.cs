using Smart.Core.Configuration;

namespace Smart.Core.Dependency
{
    /// <summary>
    /// 容器，控制对象的生命周期和对象间的关系
    /// </summary>
    public interface IContainerManager
    {
        /// <summary>
        /// 初始化组件和插件。
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(SmartConfig config);

        /// <summary>
        /// 从上下文检索服务,当serviceName不为空的时候，
        /// 使用默认的生命周期管理,
        /// 否则当注入IDependencyResolver 类型时使用IDependencyResolver的生命周期管理
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        T Resolve<T>(string serviceName = null) where T : class;
   
    }
}
