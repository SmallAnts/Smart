using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 类型查找接口
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 已加载的程序集集合
        /// </summary>
        IList<Assembly> Assemblies { get; }

        /// <summary>
        /// 通过继承关系查找类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<Type> ForTypesDerivedFrom<T>();

        /// <summary>
        /// 通过条件查找类型
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <returns></returns>
        IEnumerable<Type> ForTypesMatching(Predicate<Type> typeFilter);
    }
}
