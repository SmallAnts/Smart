using System;
using System.Collections.Generic;
using System.Threading;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 重试操作
    /// </summary>
    public static class Retry
    {

        /// <summary>
        /// 重试执行无返回值的方法
        /// </summary>
        /// <param name="action">重试执行的方法</param>
        /// <param name="when">当满足指定异常条件时执行重试操作</param>
        /// <param name="retryInterval">重试操作间隔时间</param>
        /// <param name="retryCount">重试次数</param>
        public static void TryInvoke(this Action action, Predicate<Exception> when = null, int retryInterval = 100, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    if (when == null || when(ex))
                    {
                        exceptions.Add(ex);
                        Thread.Sleep(retryInterval);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// 重试执行带返回值的方法
        /// </summary>
        /// <param name="func">重试执行的方法</param>
        /// <param name="when">当满足指定异常条件时执行重试操作</param>
        /// <param name="retryInterval">重试操作间隔时间</param>
        /// <param name="retryCount">重试次数</param>
        public static T TryInvoke<T>(this Func<T> func, Predicate<Exception> when = null, int retryInterval = 100, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    if (when == null || when(ex))
                    {
                        exceptions.Add(ex);
                        Thread.Sleep(retryInterval);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
