using Autofac;
using Smart.Core.Configuration;

namespace Smart.Core.Dependency
{
    /// <summary>
    /// 自定义依赖注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务和接口
        /// </summary>
        /// <param name="builder">容器生成器</param>
        /// <param name="config">配置对象</param>
        void Register(ContainerBuilder builder, SmartConfig config);

        /// <summary>
        /// 依赖关系注册的执行顺序
        /// </summary>
        int Order { get; }
    }
}
