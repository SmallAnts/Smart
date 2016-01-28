using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Pattern
{
    /// <summary>
    /// 简单工厂模式
    /// </summary>
    public abstract class SimpleFactory
    {
        public virtual T Create<T>(string key)
        {
            return default(T);
        }
    }
}
