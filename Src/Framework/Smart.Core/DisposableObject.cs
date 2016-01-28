using System;

namespace Smart.Core
{
    /// <summary>
    /// 实现可释放的对象基础类
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected DisposableObject()
        {
            IsDisposed = false;
        }

        /// <summary>
        /// 获取是否已经释放或重置非托管资源
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }
    }
}
