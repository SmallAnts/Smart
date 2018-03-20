using Autofac;
using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web;

namespace Smart.Core.Dependency
{
    /// <summary>
    /// 依赖解析器接口
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
    /// 当 HttpContext.Current != null 时，每次请求使用一个生命周期范围，否则每个线程使用一个生命周期范围。
    /// </summary>
    public class DefaultDependencyResolver : IDependencyResolver
    {
        Dictionary<int, ILifetimeScope> _threadLifetimeScopes;
        const int keepAlivePeriod = 60000;    //Configuration.SmartConfig.LifetimeScopeKeepAlivePeriod
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDependencyResolver()
        {
            this._threadLifetimeScopes = new Dictionary<int, ILifetimeScope>();
            this.StartCheck();
        }

        /// <summary>
        /// 生命周期范围
        /// </summary>
        protected virtual ILifetimeScope LifetimeScope
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
                else
                {
                    int threadId = NativeMethods.GetCurrentThreadId();

                    if (!_threadLifetimeScopes.TryGetValue(threadId, out ILifetimeScope threadScope))
                    {
                        threadScope = ((ContainerManager)SmartContext.Current).BeginLifetimeScope();
                        _threadLifetimeScopes.Add(threadId, threadScope);
                    }
                    return threadScope;
                }

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
                return LifetimeScope.Resolve(serviceType);
            }
            else
            {
                return LifetimeScope.ResolveNamed(serviceName, serviceType);
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
                return LifetimeScope.Resolve<T>();
            }
            else
            {
                return LifetimeScope.ResolveNamed<T>(serviceName);
            }
        }

        private void StartCheck()
        {
            if (HttpContext.Current != null) return;

            var thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(keepAlivePeriod);
                        if (_threadLifetimeScopes.Count == 0) continue;

                        var threads = Process.GetCurrentProcess().Threads;

                        #region 移除已退出线程的生命周期范围
                        for (int i = threads.Count - 1; i >= 0; i--)
                        {
                            try
                            {
                                if (threads[i].ThreadState == System.Diagnostics.ThreadState.Standby)
                                {
                                    this.DisposeScope(threads[i].Id);
                                }
                            }
                            catch (Exception ex)
                            {
#if DEBUG
                                Debug.WriteLine(ex.Message);
#endif
                            }
                        }
                        #endregion

                        #region 移除已不存在的线程的生命周期范围
                        var keys = new List<int>();
                        foreach (var item in _threadLifetimeScopes)
                        {
                            int i = threads.Count - 1;
                            for (; i >= 0; i--)
                            {
                                if (threads[i].Id == item.Key) break;
                            }
                            if (i == -1)
                            {
                                keys.Add(item.Key);
                            }
                        }

                        foreach (var key in keys)
                        {
                            this.DisposeScope(key);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void DisposeScope(int threadId)
        {
            if (_threadLifetimeScopes.TryGetValue(threadId, out ILifetimeScope scope))
            {
                _threadLifetimeScopes.Remove(threadId);
                scope.Dispose();
            }
        }

    }

    /// <summary>
    ///  当 HttpContext.Current != null 时，每次请求使用一个生命周期范围
    /// </summary>
    public class HttpDependencyResolver : DefaultDependencyResolver
    {
        /// <summary>
        /// 生命周期范围
        /// </summary>
        protected override ILifetimeScope LifetimeScope
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return base.LifetimeScope;
                }
                return ((ContainerManager)SmartContext.Current)._container;
            }
        }
    }

    /// <summary>
    ///  当 HttpContext.Current != null 时，每次请求使用一个生命周期范围
    /// </summary>
    [Obsolete("请使用 HttpDependencyResolver")]
    [Browsable(false),
    EditorBrowsable(EditorBrowsableState.Never)]
    public class HttpDependencyResover : HttpDependencyResolver
    {
    }

}
