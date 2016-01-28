using System;

namespace Smart.Core.Configuration
{
    /// <summary>
    /// 配置
    /// </summary>
    public class SmartConfig
    {
        /// <summary>
        /// 获取或设置是否忽略执行启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Infrastructure.ITypeFinder TypeFinder { get; set; }

        /// <summary>
        /// 依赖注册完成后要处理的行为，
        /// <para>如：DependencyResolver.SetResolver(new AutofacDependencyResolver(container));</para>
        /// </summary>
        public Action<Autofac.IContainer> OnDependencyRegistered { get; set; }
    }
}
