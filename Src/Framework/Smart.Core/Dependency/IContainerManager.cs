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
        /// 从上下文检索服务
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        T Resolve<T>(string serviceName = null) where T : class;
    }
}
