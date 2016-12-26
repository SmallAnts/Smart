using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WinControlExtensions
    {
        /// <summary>
        /// 启用或禁用双缓冲
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        public static void SetDoubleBuffer(this Control control, bool value = true)
        {
            if (SystemInformation.TerminalServerSession) return; //远程桌面时该值为true
            var pi = typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic | BindingFlags.Instance);
            pi?.SetValue(control, value, null);
        }
    }
}
