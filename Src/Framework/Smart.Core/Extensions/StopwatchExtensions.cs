using System;
using System.Diagnostics;

namespace Smart.Core.Extensions
{
    public static class StopwatchExtensions
    {
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="length">执行次数</param>
        /// <param name="action">执行方法</param>
        /// <returns>当前实例测量得出的总运行时间。</returns>
        public static TimeSpan Time(this Stopwatch sw, int length, Action action)
        {
            sw.Restart();
            for (int i = 0; i < length; i++)
            {
                action();
            }
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TimeSpan Time(this Stopwatch sw, int length, Action<int> action)
        {
            sw.Restart();
            for (int i = 0; i < length; i++)
            {
                action(i);
            }
            sw.Stop();
            return sw.Elapsed;
        }
    }
}
