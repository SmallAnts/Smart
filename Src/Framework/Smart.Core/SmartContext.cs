using Smart.Core.Configuration;
using Smart.Core.DependencyManagement;
using Smart.Core.Infrastructure;
using Smart.Core.Localization;
using System.Runtime.CompilerServices;

namespace Smart.Core
{
    /// <summary>
    /// 上下文访问对象（IOC容器，日志工具等）
    /// </summary>
    public class SmartContext
    {
        #region 属性

        public Localizer T { get; set; }

        #endregion

        /// <summary>
        /// 初始化一个静态实例工厂。
        /// </summary>
        /// <param name="forceRecreate">创建一个新工厂实例，即使工厂已被初始化。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainerManager Initialize(SmartConfig config = null, bool forceRecreate = false)
        {
            if (Singleton<IContainerManager>.Instance == null || forceRecreate)
            {
                var container = new ContainerManager();
                container.Initialize(config);
                Singleton<IContainerManager>.Instance = container;
            }
            return Singleton<IContainerManager>.Instance;
        }

        /// <summary>
        /// 替换容器，实现自定义容器
        /// </summary>
        /// <param name="containerManager">容器</param>
        /// <remarks></remarks>
        public static void Replace(IContainerManager containerManager)
        {
            Singleton<IContainerManager>.Instance = containerManager;
        }

        /// <summary>
        /// 获取当前容器实例
        /// </summary>
        public static IContainerManager Current
        {
            get
            {
                if (Singleton<IContainerManager>.Instance == null)
                {
                    Initialize(null, false);
                }
                return Singleton<IContainerManager>.Instance;
            }
        }

    }
}
