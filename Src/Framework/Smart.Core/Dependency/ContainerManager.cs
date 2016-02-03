using Autofac;
using Smart.Core.Configuration;
using Smart.Core.Extensions;
using Smart.Core.Infrastructure;
using System.Linq;

namespace Smart.Core.Dependency
{
    /// <summary>
    /// 默认IOC容器（控制对象的生命周期和对象间的关系）（基于Autofac实现）
    /// </summary>
    public class ContainerManager : IContainerManager
    {
        #region 私有字段

        internal Autofac.IContainer _container;

        #endregion

        #region 私有方法

        /// <summary>
        /// 运行启动时的初始化任务
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            var typeFinder = _container.Resolve<ITypeFinder>();
            var startUpTasks = typeFinder.ForTypesDerivedFrom<IStartupTask>()
                .ToInstances<IStartupTask>()
                .OrderBy(s => s.Order)
                .ToList();
            foreach (var startUpTask in startUpTasks)
            {
                startUpTask.Execute();
            }
        }

        /// <summary>
        /// 依赖注册
        /// </summary>
        /// <param name="config">配置对象</param>
        protected virtual void RegisterDependencies(SmartConfig config)
        {
            // 初始化容器
            var builder = new Autofac.ContainerBuilder();
            this._container = builder.Build();

            #region 注册依赖实例

            builder = new Autofac.ContainerBuilder();
            builder.RegisterInstance(config.TypeFinder).As<ITypeFinder>().SingleInstance();
            builder.RegisterInstance(config).As<SmartConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IContainerManager>().SingleInstance();
            builder.RegisterType<Caching.HttpCache>().Named<Caching.ICache>("smart.lang").SingleInstance();
            builder.Update(this._container);

            #endregion

            #region 注册实现 IDependency 接口的依赖类型

            builder = new Autofac.ContainerBuilder();
            var dependencies = config.TypeFinder.ForTypesDerivedFrom<IDependency>().ToList();
            foreach (var dependency in dependencies)
            {
                var reg = builder.RegisterType(dependency)
                    .AsImplementedInterfaces() // 注册所有接口
                    .PropertiesAutowired(); // 自动属性注入
                if (typeof(ISingletonDependency).IsAssignableFrom(dependency))
                {
                    reg.SingleInstance();
                }
                else if (typeof(ITransientDependency).IsAssignableFrom(dependency))
                {
                    reg.InstancePerDependency();
                }
                else
                {
                    reg.InstancePerLifetimeScope();
                }
            }
            builder.Update(this._container);

            #endregion

            #region 注册其他程序集所提供的依赖关系（实现 IDependencyRegistrar 接口的类）

            builder = new ContainerBuilder();
            var drInstances = config.TypeFinder.ForTypesDerivedFrom<IDependencyRegistrar>()
                .ToInstances<IDependencyRegistrar>()
                .OrderBy(t => t.Order)
                .ToList();
            foreach (var dependencyRegistrar in drInstances)
            {
                dependencyRegistrar.Register(builder, config);
            }
            builder.Update(this._container);

            #endregion

            // 注册完成后处理
            if (config.OnDependencyRegistered != null)
            {
                config.OnDependencyRegistered(this._container);
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 初始化组件和插件。
        /// </summary>
        /// <param name="config">配置对象</param>
        public void Initialize(SmartConfig config)
        {
            // 设置配置文件
            if (config == null)
            {
                config = new SmartConfig();
            }
            // 注册依赖
            RegisterDependencies(config);

            // 启动时运行的任务
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        /// <summary>
        /// 从上下文检索服务
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public T Resolve<T>(string serviceName = null) where T : class
        {
            if (string.IsNullOrEmpty(serviceName))
                return _container.Resolve<T>();
            else
                return _container.ResolveNamed<T>(serviceName);
        }

        #endregion
    }
}
