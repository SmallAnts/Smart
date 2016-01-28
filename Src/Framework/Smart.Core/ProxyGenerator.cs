using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Smart.Core
{
    /// <summary>
    /// 动态生成代理类
    /// </summary>
    public static class ProxyGenerator
    {
        /// <summary>
        /// 运行时类型句柄缓存列表
        /// </summary>
        static ConcurrentDictionary<string, RuntimeTypeHandle> typeHandles
           = new ConcurrentDictionary<string, RuntimeTypeHandle>();

        /// <summary>
        /// 根据拦截器类型动态生成代理类并缓存
        /// </summary>
        /// <typeparam name="T">需要注入拦截器的对象类型</typeparam>
        /// <param name="interceptor">拦截器</param>
        /// <returns>注入拦截器的代理类</returns>
        public static T CreateInstanceByCache<T>(IInterceptor interceptor) where T : class
        {
            var cacheKey = typeof(T).FullName + "Proxy";
            RuntimeTypeHandle handle;
            if (typeHandles.TryGetValue(cacheKey, out handle))
            {
                var type = Type.GetTypeFromHandle(handle);
                return Activator.CreateInstance(type, interceptor) as T;
            }
            else
            {
                var t = CreateInstance<T>(interceptor);
                typeHandles.TryAdd(cacheKey, t.GetType().TypeHandle);
                return t;
            }
        }

        /// <summary>
        /// 根据拦截器类型动态生成代理类
        /// </summary>
        /// <typeparam name="T">需要注入拦截器的对象类型</typeparam>
        /// <param name="interceptor">拦截器</param>
        /// <returns>注入拦截器的代理类</returns>
        public static T CreateInstance<T>(IInterceptor interceptor) where T : class
        {
            var typeBuilder = GetTypeBuilder<T>();
            InjectionOfInterceptor<T>(typeBuilder, interceptor);
            return CreateInstance<T>(typeBuilder, interceptor);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearCache()
        {
            typeHandles.Clear();
        }

        /// <summary>
        /// 生成注入拦截器的代理方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeBuilder"></param>
        /// <param name="interceptor"></param>
        private static void InjectionOfInterceptor<T>(TypeBuilder typeBuilder, IInterceptor interceptor)
        {
            if (typeBuilder == null)
                throw new ArgumentNullException("typeBuilder");

            if (interceptor == null)
                throw new ArgumentNullException("interceptor");

            var sourceType = typeof(T);
            var interceptorType = typeof(IInterceptor); // 拦截器类型
            var invocationType = typeof(Invocation);    // 调用信息对象类型

            #region 定义拦截器字段
            var fieldInterceptor = typeBuilder.DefineField("_interceptor", interceptorType, FieldAttributes.Private);
            #endregion

            #region 定义无参构造函数并初始化拦截器
            //无参构造函数数初始化
            //var constructorBuilder  = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            //var ilOfCtor = constructorBuilder.GetILGenerator();
            //ilOfCtor.Emit(OpCodes.Ldarg_0);
            //ilOfCtor.Emit(OpCodes.Newobj, interceptorType.GetConstructor(Type.EmptyTypes));
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { interceptorType });
            var ilOfCtor = constructorBuilder.GetILGenerator();
            //初始化拦截器字段 this._interceptor = interceptor;
            ilOfCtor.Emit(OpCodes.Ldarg_0); // 把this指针装入堆栈
            ilOfCtor.Emit(OpCodes.Ldarg_1); // 把构造函数中的第一个参数装入堆栈
            ilOfCtor.Emit(OpCodes.Stfld, fieldInterceptor);// 将堆栈上的值存储到字段 fieldInterceptor 中
            ilOfCtor.Emit(OpCodes.Ret);       // 函数执行结束并返回
            #endregion

            #region 定义注入拦截器的方法

            var methodsOfType = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methodsOfType)
            {
                if (method.DeclaringType == typeof(object) || !(method.IsVirtual || method.IsAbstract)) continue;
                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                #region 获取方法生成器
                var methodAttrs = MethodAttributes.Public | MethodAttributes.Virtual;
                if (sourceType.IsInterface) methodAttrs |= MethodAttributes.NewSlot;

                var methodBuilder = typeBuilder.DefineMethod(
                    method.Name,
                    methodAttrs,
                    CallingConventions.Standard,
                    method.ReturnType,
                    methodParameterTypes);

                var ilOfMethod = methodBuilder.GetILGenerator();
                #endregion

                #region 初始化调用信息变量
                //Invocation invocation = new Invocation();
                LocalBuilder invocation = ilOfMethod.DeclareLocal(invocationType); //定义局部变量 invocation
                ilOfMethod.Emit(OpCodes.Newobj, invocationType.GetConstructor(Type.EmptyTypes));
                ilOfMethod.Emit(OpCodes.Stloc, invocation);   //将堆栈上的值存储到局部变量 invocation 中

                #region 设置 invocation.Source
                if (!sourceType.IsInterface)
                {
                    // invocation.Source = new TClass();
                    ilOfMethod.Emit(OpCodes.Ldloc, invocation); // 加载局部变量 invocation 装入堆栈
                    ilOfMethod.Emit(OpCodes.Newobj, sourceType.GetConstructor(Type.EmptyTypes));
                    ilOfMethod.EmitCall(OpCodes.Call, invocationType.GetMethod("set_Source"), new[] { typeof(object) });
                }
                #endregion

                #region 设置 invocation.MethodName
                ilOfMethod.Emit(OpCodes.Ldloc, invocation);
                ilOfMethod.Emit(OpCodes.Ldstr, method.Name);
                ilOfMethod.EmitCall(OpCodes.Call, invocationType.GetMethod("set_MethodName"), new[] { typeof(string) });
                #endregion

                #region 设置 invocation.Parameters
                // tmpParameters=new object[length];
                var tmpParameters = ilOfMethod.DeclareLocal(typeof(object[]));
                ilOfMethod.Emit(OpCodes.Ldc_I4, methodParameterTypes.Length);
                ilOfMethod.Emit(OpCodes.Newarr, typeof(object));
                ilOfMethod.Emit(OpCodes.Stloc, tmpParameters);
                for (int i = 0; i < methodParameterTypes.Length; ++i)
                {
                    ilOfMethod.Emit(OpCodes.Ldloc, tmpParameters); // 把局部变量（数组）装入堆栈
                    ilOfMethod.Emit(OpCodes.Ldc_I4, i);                      // 把常量 i 装入堆栈（下标）
                    ilOfMethod.Emit(OpCodes.Ldarg, i + 1);                 // 把方法中的第 i+1 个参数装入堆栈, 参数索引从1开始
                    if (methodParameterTypes[i].IsValueType) ilOfMethod.Emit(OpCodes.Box, methodParameterTypes[i]); // 值类型装箱
                    ilOfMethod.Emit(OpCodes.Stelem_Ref);                 //  把值存储到数组的 i 索引位置
                }
                // invocation.Parameters=
                ilOfMethod.Emit(OpCodes.Ldloc, invocation);
                ilOfMethod.Emit(OpCodes.Ldloc, tmpParameters);
                ilOfMethod.EmitCall(OpCodes.Call, invocationType.GetMethod("set_Parameters"), new[] { typeof(object[]) });
                #endregion

                #endregion

                #region 调用拦截器的方法
                // this._interceptor.Intercept(invocation);
                ilOfMethod.Emit(OpCodes.Ldarg_0);   // 加载this
                ilOfMethod.Emit(OpCodes.Ldfld, fieldInterceptor);
                ilOfMethod.Emit(OpCodes.Ldloc, invocation);
                ilOfMethod.Emit(OpCodes.Call, interceptorType.GetMethod("Intercept"));

                if (method.ReturnType != typeof(void))
                {
                    ilOfMethod.Emit(OpCodes.Ldloc, invocation);
                    ilOfMethod.EmitCall(OpCodes.Call, invocationType.GetMethod("get_ReturnValue"), new[] { typeof(object) });
                    if (method.ReturnType.IsValueType)
                    {
                        ilOfMethod.Emit(OpCodes.Unbox_Any, method.ReturnType);  // 如果是值类型，拆箱 int = (int)object;
                    }
                    else
                    {
                        ilOfMethod.Emit(OpCodes.Castclass, method.ReturnType);      // 如果是引用类型，转换 Class = object as Class
                    }
                }
                // 完成并返回
                ilOfMethod.Emit(OpCodes.Ret);
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 获取代理类生成器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aba"></param>
        /// <returns></returns>
        private static TypeBuilder GetTypeBuilder<T>(AssemblyBuilderAccess aba = AssemblyBuilderAccess.Run) where T : class
        {
            var sourceType = typeof(T);
            var nameOfAssembly = sourceType.Name + "ProxyAssembly";
            var nameOfModule = sourceType.Name + "ProxyModule";
            var nameOfType = sourceType.Name + "Proxy";
            var assemblyName = new AssemblyName(nameOfAssembly); // 程序集名称
            // 定义程序集
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, aba);
            // 定义动态模块
            var moduleBuilder = aba == AssemblyBuilderAccess.RunAndSave || aba == AssemblyBuilderAccess.Save
                ? assembly.DefineDynamicModule(nameOfModule, nameOfAssembly + ".dll")
                : assembly.DefineDynamicModule(nameOfModule, false);
            // 定义类生成器
            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public);
            // 设置代理类的继承类型
            if (sourceType.IsInterface) typeBuilder.AddInterfaceImplementation(sourceType);
            else typeBuilder.SetParent(sourceType);
            return typeBuilder;
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static T CreateInstance<T>(TypeBuilder typeBuilder, params object[] args) where T : class
        {
            var t = typeBuilder.CreateType();
            return (T)Activator.CreateInstance(t, args);
        }
    }

    /// <summary>
    /// 拦截器接口
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation">调用信息对象</param>
        void Intercept(Invocation invocation);
    }

    /// <summary>
    ///  拦截器调用对象
    /// </summary>
    public class Invocation
    {
        /// <summary>
        /// 被代理的源对象
        /// </summary>
        public object Source { get; set; }
        /// <summary>
        /// 当前调用的方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 当前调用方法的参数
        /// </summary>
        public object[] Parameters { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 执行被调用的方法
        /// </summary>
        /// <returns></returns>
        public object Invoke()
        {
            if (Source == null) return null;
            var method = Source.GetType().GetMethod(MethodName);
            if (method == null) throw new Exception("不能从类型 Source 中获取到方法 " + MethodName);
            this.ReturnValue = method.Invoke(Source, Parameters);
            //if (method.ReturnType != typeof(void) && this.ReturnValue == null && method.ReturnType.IsValueType)
            //{
            //    this.ReturnValue = Activator.CreateInstance(method.ReturnType);
            //}
            return this.ReturnValue;
        }
    }
}
