using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Sample.Services
{
    /// <summary>
    /// 服务层的自定义异常信息类
    /// </summary>
    public class ServiceException : Exception
    {
        private string ErrorCode { get; set; }
        public ServiceException() : base() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
