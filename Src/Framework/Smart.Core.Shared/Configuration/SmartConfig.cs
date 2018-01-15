using System;
using System.Configuration;
using Smart.Core.Extensions;

namespace Smart.Core.Configuration
{
    /// <summary>
    /// 配置
    /// </summary>
    public class SmartConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartConfig()
        {
            var displayMiniprofiler = ConfigurationManager.AppSettings["Smart:DisplayMiniProfiler"];
            if (displayMiniprofiler != null)
            {
                this.DisplayMiniProfiler = displayMiniprofiler.AsBool();
            }
            this.Language = ConfigurationManager.AppSettings["Smart:Language"];
        }

        /// <summary>
        /// 获取或设置主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 获取或设置语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 获取或设置是否忽略执行启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// 获取或设置是否显示 MiniProfiler
        /// </summary>
        public bool DisplayMiniProfiler { get; set; }

        private Infrastructure.ITypeFinder _typeFinder;
        /// <summary>
        /// 获取或设置依赖注入类型查找工具类
        /// </summary>
        public Infrastructure.ITypeFinder TypeFinder
        {
            get { return _typeFinder ?? (_typeFinder = new Infrastructure.DirectoryTypeFinder()); }
            set { _typeFinder = value; }
        }

        /// <summary>
        /// 获取或设置依赖注册完成后要处理的行为，
        /// <para>如：DependencyResolver.SetResolver(new AutofacDependencyResolver(container));</para>
        /// </summary>
        public Action<Autofac.IContainer> OnDependencyRegistered { get; set; }
    }
}
