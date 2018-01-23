using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
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
        private static AssemblyBuilder _assemblyBuilder;
        private static ModuleBuilder _moduleBuilder;

        /// <summary>
        /// 运行时类型句柄缓存列表
        /// </summary>
        static ConcurrentDictionary<string, Type> types
           = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// 运行时类型句柄缓存列表
        /// </summary>
        static ConcurrentBag<Type> extendTypes
           = new ConcurrentBag<Type>();

        static ProxyGenerator()
        {
            var nameOfAssembly = "SmartProxyAssembly";
            var nameOfModule = "SmartProxyModule";
            var assemblyName = new AssemblyName(nameOfAssembly); // 程序集名称
            // 定义程序集
            var aba = AssemblyBuilderAccess.RunAndCollect;
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, aba);
            // 定义动态模块
            _moduleBuilder = aba == AssemblyBuilderAccess.RunAndSave || aba == AssemblyBuilderAccess.Save
                ? _assemblyBuilder.DefineDynamicModule(nameOfModule, nameOfAssembly + ".dll")
                : _assemblyBuilder.DefineDynamicModule(nameOfModule, false);
        }
        /// <summary>
        /// 根据拦截器类型动态生成代理类并缓存
        /// </summary>
        /// <typeparam name="T">需要注入拦截器的对象类型</typeparam>
        /// <param name="interceptor">拦截器</param>
        /// <returns>注入拦截器的代理类</returns>
        public static T CreateInstanceByCache<T>(IInterceptor interceptor) where T : class
        {
            var cacheKey = typeof(T).FullName + "Proxy";
            if (types.TryGetValue(cacheKey, out Type type))
            {
                return Activator.CreateInstance(type, interceptor) as T;
            }
            else
            {
                var instance = CreateInstance<T>(interceptor);
                types.TryAdd(cacheKey, instance.GetType());
                return instance;
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
            typeBuilder.InjectionOfInterceptor<T>(interceptor);
            return typeBuilder.CreateInstance<T>(interceptor);
        }

        /// <summary>
        /// 创建动态对象
        /// </summary>
        /// <param name="extendObject"></param>
        /// <returns></returns>
        public static Data.ExtendObject CreateExtendObject(object extendObject)
        {
            var type = CreateType(extendObject);
            var instance = Activator.CreateInstance(type);
            var pis = extendObject.GetType().GetProperties();
            foreach (var pi in pis)
            {
                type.GetProperty(pi.Name).SetValue(instance, pi.GetValue(extendObject, null), null);
            }
            return (Data.ExtendObject)instance;
        }

        /// <summary>
        ///  创建动态对象
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static Data.ExtendObject CreateExtendObject(IEnumerable<ProxyPropertyInfo> properties)
        {
            var type = CreateType(properties);
            var instance = Activator.CreateInstance(type);
            foreach (var pi in properties)
            {
                if (pi.Value != null)
                {
                    type.GetProperty(pi.Name).SetValue(instance, pi.Value, null);
                }
            }
            return (Data.ExtendObject)instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendObject"></param>
        /// <returns></returns>
        private static Type CreateType(object extendObject)
        {
            if (extendObject == null) return typeof(Data.ExtendObject);
            var pis = extendObject.GetType().GetProperties();
            var type = GetExtendTypeByCache(pis);
            if (type != null)
            {
                return type;
            }
            else
            {
                var properties = pis
                      .Select(p => new ProxyPropertyInfo(p.Name, p.PropertyType, p.GetValue(extendObject, null)))
                      .ToList();
                type = CreateType(properties);
                extendTypes.Add(type);
                return type;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static Type CreateType(IEnumerable<ProxyPropertyInfo> properties)
        {
            if (properties == null) return typeof(Data.ExtendObject);

            var typeBuilder = GetTypeBuilder<Data.ExtendObject>();

            #region 定义无参构造函数并初始化字段

            foreach (var pi in properties)
            {
                typeBuilder.AddProperty(pi);
            }

            #endregion
            return typeBuilder.CreateType();
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearCache()
        {
            types.Clear();
        }

        /// <summary>
        /// 获取代理类生成器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aba"></param>
        /// <returns></returns>
        private static TypeBuilder GetTypeBuilder<T>(AssemblyBuilderAccess aba = AssemblyBuilderAccess.RunAndCollect) where T : class
        {
            var sourceType = typeof(T);
            var nameOfType = $"{sourceType.Name}_{DateTime.Now.Ticks}";
            // 定义类生成器
            var typeBuilder = _moduleBuilder.DefineType(nameOfType, TypeAttributes.Public);
            // 设置代理类的继承类型
            if (sourceType.IsInterface)
            {
                typeBuilder.AddInterfaceImplementation(sourceType);
            }
            else
            {
                typeBuilder.SetParent(sourceType);
            }
            return typeBuilder;
        }

        private static Type GetExtendTypeByCache(PropertyInfo[] pis)
        {
            foreach (var item in extendTypes)
            {
                bool isTheType = true;
                var extendPis = item.GetProperties();
                if (extendPis.Length != pis.Length) continue;
                foreach (var pi in extendPis)
                {
                    if (pis.FirstOrDefault(p => p.Name == pi.Name && p.PropertyType == pi.PropertyType) == null)
                    {
                        isTheType = false;
                        break;
                    }
                }
                if (isTheType) return item;
            }
            return null;
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

    /// <summary>
    /// 
    /// </summary>
    public static class TypeBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyInfo"></param>
        public static void AddProperty(this TypeBuilder typeBuilder, ProxyPropertyInfo propertyInfo, FieldBuilder field = null)
        {
            #region 定义字段

            if (field == null)
            {
                field = typeBuilder.DefineField("_" + propertyInfo.Name, propertyInfo.Type, FieldAttributes.Private);
                if (propertyInfo.Value != null)
                {
                    field.SetConstant(propertyInfo.Value);
                }
            }
            #endregion

            #region 构造属性get方法 

            var methodGet = typeBuilder.DefineMethod(
                "get" + propertyInfo.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyInfo.Type, Type.EmptyTypes);

            // return this._filedName;
            var ilOfGet = methodGet.GetILGenerator();
            ilOfGet.Emit(OpCodes.Ldarg_0);
            ilOfGet.Emit(OpCodes.Ldfld, field);
            ilOfGet.Emit(OpCodes.Ret);

            #endregion

            #region 构造属性set方法

            var methodSet = typeBuilder.DefineMethod(
                "set" + propertyInfo.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { propertyInfo.Type });
            // this._filedName = value;
            var ilOfSet = methodSet.GetILGenerator();
            ilOfSet.Emit(OpCodes.Ldarg_0);
            ilOfSet.Emit(OpCodes.Ldarg_1);
            ilOfSet.Emit(OpCodes.Stfld, field);
            ilOfSet.Emit(OpCodes.Ret);

            #endregion

            var property = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.None, propertyInfo.Type, null);
            property.SetGetMethod(methodGet);
            property.SetSetMethod(methodSet);
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this TypeBuilder typeBuilder, params object[] args) where T : class
        {
            var t = typeBuilder.CreateType();
            return (T)Activator.CreateInstance(t, args);
        }

        /// <summary>
        /// 生成注入拦截器的代理方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeBuilder"></param>
        /// <param name="interceptor"></param>
        public static void InjectionOfInterceptor<T>(this TypeBuilder typeBuilder, IInterceptor interceptor)
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
    }

    public class ProxyPropertyInfo
    {
        public ProxyPropertyInfo(string name, Type type, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public object Value { get; private set; }
    }
}
