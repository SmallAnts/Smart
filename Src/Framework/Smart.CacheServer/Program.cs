using System;
using System.Threading;
using System.Windows.Forms;

namespace Smart.CacheServer
{
    static class Program
    {
        static Mutex mutex;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            mutex = new Mutex(true, Application.ProductName, out bool createdNew);
            if (!createdNew) // 进程已经存在！
            {
                Mutex.OpenExisting(Application.ProductName);
                Application.Exit();
                return;
            }
            mutex.ReleaseMutex();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException((Exception)e.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledException(e.Exception);
        }

        private static void UnhandledException(Exception ex)
        {
            MessageBox.Show(
                $"发生捕获的异常，错误信息：{ (ex.InnerException ?? ex).Message}",
                "错误信息",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
