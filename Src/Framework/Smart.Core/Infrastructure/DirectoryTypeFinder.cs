using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 根据指定目录路径中的 DLL 文件查找类型。
    /// </summary>
    public class DirectoryTypeFinder : ITypeFinder
    {
        private string assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease|^d3dcompiler_43|^icudt|^libcef|^libegl|^libglesv2|^sqlite.interop|^widevinecdmadapter";
        private IList<Assembly> _assemblies;
        /// <summary>
        /// 程序集列表
        /// </summary>
        public IList<Assembly> Assemblies
        {
            get { return _assemblies ?? (_assemblies = new List<Assembly>()); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="fileFilter"></param>
        /// <param name="searchOption"></param>
        public DirectoryTypeFinder(string path = null, string searchPattern = "*.dll", Predicate<string> fileFilter = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = GetBinDirectory();
            }
            var files = new DirectoryInfo(path).GetFiles(searchPattern, searchOption);

            foreach (var file in files)
            {
                var filename = file.FullName.ToLower();
                if (fileFilter == null || fileFilter(filename))
                {
                    var assembly = LoadAssembly(filename);
                    if (assembly == null) continue;
                    if (!Matches(assembly.FullName, assemblySkipLoadingPattern))
                    {
                        Assemblies.Add(assembly);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<Type> ForTypesDerivedFrom<T>()
        {
            foreach (var assembly in this.Assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (typeof(T) != type && !type.IsAbstract && typeof(T).IsAssignableFrom(type)) yield return type;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <returns></returns>
        public IEnumerable<Type> ForTypesMatching(Predicate<Type> typeFilter)
        {
            foreach (var assembly in this.Assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (typeFilter != null && typeFilter(type)) yield return type;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                return HttpRuntime.BinDirectory;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private static Assembly LoadAssembly(string codeBase)
        {
            AssemblyName assemblyName;
            try
            {
                assemblyName = AssemblyName.GetAssemblyName(codeBase);
            }
            catch (ArgumentException)
            {
                assemblyName = new AssemblyName();
                assemblyName.CodeBase = codeBase;
            }
            catch (Exception ex)
            {
                Log("Smart.Core.Infrastructure.LoadAssembly: " + ex.Message);
                return null;
            }
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                Log("Smart.Core.Infrastructure.LoadAssembly: " + ex.Message);
                return null;
                //throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log\\log_" + DateTime.Today.ToString("yyyyMMdd") + ".txt");
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.AppendAllText(file, DateTime.Now.ToString() + " > " + message + Environment.NewLine);
        }
    }
}


