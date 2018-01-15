using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Extensions
{
    public static partial class TaskExtensions
    {
#if NET40
        /// <summary>
        /// 创建指定结果、成功完成的 Task<TResult>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            return new Task<TResult>(() => result);
        }
#endif
    }
}
