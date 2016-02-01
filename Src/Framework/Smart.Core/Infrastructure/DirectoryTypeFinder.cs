using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Smart.Core.Extensions;
using System.Text.RegularExpressions;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 根据指定目录路径中的 DLL 文件查找类型。
    /// </summary>
    public class DirectoryTypeFinder : ITypeFinder
    {
        private string assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";
        private IList<Assembly> _assemblies;
        public IList<Assembly> Assemblies
        {
            get { return _assemblies ?? (_assemblies = new List<Assembly>()); }
        }

        public DirectoryTypeFinder(string path = null, string searchPattern = "*.dll", Predicate<string> fileFilter = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = GetBinDirectory();
            }
            var files = new DirectoryInfo(path).GetFiles(searchPattern);

            foreach (var file in files)
            {
                var filename = file.FullName.ToLower();
                if (fileFilter == null || fileFilter(filename))
                {
                    var assembly = LoadAssembly(filename);
                    if (!Matches(assembly.FullName, assemblySkipLoadingPattern))
                    {
                        Assemblies.Add(assembly);
                    }
                }
            }
        }

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
            return Assembly.Load(assemblyName);
        }

    }
}
