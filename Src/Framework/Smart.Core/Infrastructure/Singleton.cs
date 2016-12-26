using System;
using System.Collections.Generic;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 为指定类型存储一个单例实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : Singleton
    {
        static T instance;

        /// <summary>
        ///指定类型的单例实例
        /// </summary>
        public static T Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                // 将实例放入字典
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    /// <summary>
    ///  单列容器 <see cref="Singleton{T}"/>.
    /// </summary>
    public class Singleton
    {
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        static readonly IDictionary<Type, object> allSingletons;

        /// <summary>
        /// 从静态字典中获取指定单例对象
        /// </summary>
        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }
    }
}
