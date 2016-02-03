namespace Smart.Core.Dependency
{
    /// <summary>
    /// <para>实现该接口的类将自动注册到IOC容器中</para>
    /// 在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// <para>实现该接口的类将自动注册到IOC容器中</para>
    /// 只创建一个依赖实例（单例）.
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// <para>实现该接口的类将自动注册到IOC容器中</para>
    /// 对每一个依赖或每一次调用创建一个新的唯一的实例。
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }

}
