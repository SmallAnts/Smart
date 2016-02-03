using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Smart.Core.Configuration;
using Autofac;
using Smart.Core;
using System;
using System.Threading;

namespace Smart.Tests
{
    [TestClass]
    public class SmartContextTest
    {
        [TestMethod]
        public void TestInit()
        {
            SmartContext.Initialize();
            var t0 = SmartContext.Current.Resolve<TestClass>();
            var t1 = SmartContext.Current.Resolve<TestClass2>();
            var t2 = SmartContext.Current.Resolve<IT<TestClass>>();
            var t3 = SmartContext.Current.Resolve<IT<TestClass2>>();
            var t4 = SmartContext.Current.Resolve<IT<TestClass, TestClass2>>();

            var s1 = SmartContext.Current.Resolve<IService1>();
            var s2 = SmartContext.Current.Resolve<IService2>();
            var s3 = SmartContext.Current.Resolve<IService3>();
            Thread.Sleep(2000);
            var s11 = SmartContext.Current.Resolve<IService1>();
            var s22 = SmartContext.Current.Resolve<IService2>();
            var s33 = SmartContext.Current.Resolve<IService3>();
        }
    }

    class DemoStartupTask : Core.Infrastructure.IStartupTask
    {
        public int Order { get { return 0; } }

        public void Execute()
        {
            Trace.Write("初始化任务被执行了...");
        }
    }

    class DemoDependencyRegister : Core.Dependency.IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(Autofac.ContainerBuilder builder, SmartConfig config)
        {
            builder.RegisterGeneric(typeof(ITmpl<>)).As(typeof(IT<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ITmpl<,>)).As(typeof(IT<,>)).InstancePerLifetimeScope();
            builder.RegisterType<TestClass>().AsSelf().PropertiesAutowired().SingleInstance();
            builder.RegisterType<TestClass2>().AsSelf().PropertiesAutowired().SingleInstance();
        }
    }

    class TestClass
    {
        public IService1 Service { get; set; }
    }
    class TestClass2
    {
        public IT<TestClass> Service { get; set; }
    }
    interface IT<T>
    {
    }
    interface IT<T1, T2>
    {
    }
    class ITmpl<T> : IT<T>
    {
        public T Entity { get; set; }
    }
    class ITmpl<T1, T2> : IT<T1, T2>
    {
        public T1 Entity1 { get; set; }
        public T1 Entity2 { get; set; }
    }
    interface IService1 : Core.Dependency.IDependency
    {
    }
    interface IService2 : Core.Dependency.ISingletonDependency
    {
    }
    interface IService3 : Core.Dependency.ITransientDependency
    {
    }
    abstract class ServiceBase
    {
        string test = string.Empty;
        public ServiceBase()
        {
            test = DateTime.Now.ToString();
        }
        public override string ToString()
        {
            return test;
        }
    }
    class Service1 : ServiceBase, IService1
    {
        public Service1() : base() { }
    }
    class Service2 : ServiceBase, IService2
    {
        public Service2() : base() { }
    }
    class Service3 : ServiceBase, IService3
    {
        public Service3() : base() { }
    }
}