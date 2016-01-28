using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Smart.Core.Extensions;

namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 根据指定目录路径中的 DLL 文件查找类型。
    /// </summary>
    public class DirectoryTypeFinder : ITypeFinder
    {
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
                    Assemblies.Add(LoadAssembly(filename));
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
