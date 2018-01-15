using System;
using System.Diagnostics;
using System.Text;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProcessExtensions
    {
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_SHOW = 5;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;

        /// <summary>
        /// 激活指定进程的主窗体并给予它焦点。
        /// </summary>
        /// <param name="process">进程</param>
        /// <param name="match">条件，如果有条件参数，可以为相同进程名的进程进行过滤，否则，只激活当前进程</param>
        public static void Activate(this Process process, Predicate<Process> match = null)
        {
            var ps = Process.GetProcessesByName(process.ProcessName);
            foreach (var p in ps)
            {
                if (match == null || match(p))
                {
                    NativeMethods.ShowWindowAsync(p.MainWindowHandle, SW_NORMAL);     // 调用api函数，正常显示窗口 
                    NativeMethods.SetForegroundWindow(p.MainWindowHandle);   // 将窗口放置最前端。
                    break;
                }
            }

        }

        /// <summary>
        /// 向指定进程的主窗口发送消息
        /// </summary>
        /// <param name="process">接收消息的进程</param>
        /// <param name="flag"></param>
        /// <param name="message"></param>
        public static void SendMessage(this Process process, int flag, string message)
        {
            // 设置消息结构体
            var data = new NativeMethods.CopyDataStruct
            {
                dwData = (IntPtr)flag
            };
            if (message != null)
            {
                int len = Encoding.Default.GetBytes(message).Length;
                data.lpData = message;
                data.cbData = len + 1;
            }
            NativeMethods.SendMessage(process.MainWindowHandle, (int)NativeMethods.WindowsMessage.WM_COPYDATA, 0, ref data);
        }

        /// <summary>
        /// 向指定句柄窗口发送消息
        /// </summary>
        /// <param name="handle">接收消息的窗口句柄</param>
        /// <param name="flag"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IntPtr handle, int flag, string message)
        {
            // 设置消息结构体
            var data = new NativeMethods.CopyDataStruct
            {
                dwData = (IntPtr)flag
            };
            if (message != null)
            {
                int len = Encoding.Default.GetBytes(message).Length;
                data.lpData = message;
                data.cbData = len + 1;
            }
            NativeMethods.SendMessage(handle, (int)NativeMethods.WindowsMessage.WM_COPYDATA, 0, ref data);
        }
    }
}
