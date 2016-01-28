using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Pattern
{

    /// <summary>
    /// 装饰组件接口
    /// </summary>
    public interface IDecorativeComponent
    {
        void Execute();
    }

    /// <summary>
    /// 装饰组件抽象类
    /// <para>在以下情况下应当使用装饰模式</para>
    /// 1.	需要扩展一个类的功能，或给一个类增加附加责任。 
    /// 2.	需要动态地给一个对象增加功能，这些功能可以再动态地撤销。 
    /// 3.	需要增加由一些基本功能的排列组合而产生的非常大量的功能，从而使继承关系变得不现实。
    /// </summary>
    public abstract class Decorator : IDecorativeComponent
    {
        IDecorativeComponent _component;
        public void SetComponent(IDecorativeComponent component)
        {
            _component = component;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute()
        {
            if (_component != null)
            {
                _component.Execute();
            }
        }
    }
}
