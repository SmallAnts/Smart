﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WinControlExtensions
    {
        static PropertyInfo OwnerPI;

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

        /// <summary>
        /// 在拥有此控件的基础窗口句柄的线程上执行指定的委托。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void Invoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// 在拥有此控件的基础窗口句柄的线程上异步执行指定的委托。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void BeginInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// 获取分配控件
        /// </summary>
        /// <param name="control"></param>
        public static Control GetAccessibilityObjectOwner(this Control control)
        {
            if (control?.AccessibilityObject == null) return control;
            if (OwnerPI == null)
            {
                OwnerPI = control.AccessibilityObject.GetType().GetProperty("Owner");
            }
            return (Control)OwnerPI.GetValue(control.AccessibilityObject, null);
        }

        /// <summary>
        /// 获取焦点所在控件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Control GetFocusControl(this Form form)
        {
            var intptr = NativeMethods.GetFocus();
            if (intptr != IntPtr.Zero)
            {
                return Control.FromHandle(intptr);
            }
            return null;
        }
    }
}
