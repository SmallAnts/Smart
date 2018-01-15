using System;

namespace Smart.Core
{
    /// <summary>
    /// 表示在应用程序执行期间发生的错误
    /// </summary>
    [Serializable]
    public class SmartException : Exception
    {
        /// <summary>
        /// 初始化的异常类的一个新实例。
        /// </summary>
        public SmartException() { }

        /// <summary>
        /// 使用指定的错误信息初始化异常类的一个新实例。
        /// </summary>
        /// <param name="message">错误描述信息</param>
        public SmartException(string message) : base(message) { }

        /// <summary>
        /// 使用指定的错误信息初始化异常类的一个新实例。
        /// </summary>
		/// <param name="messageFormat">异常消息格式。</param>
		/// <param name="args">异常消息格式参数。</param>
        public SmartException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args)) { }

        /// <summary>
        /// 使用指定的错误信息和参考，是此异常原因的内部异常初始化异常类的一个新实例。
        /// </summary>
        /// <param name="message">错误描述信息</param>
        /// <param name="innerException">如果没有指定内部异常，则是当前异常的原因，或是空引用。</param>
        public SmartException(string message, Exception innerException) : base(message, innerException) { }
    }
}
