using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;

namespace Smart.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        public TestBase()
        {
            var data_path = Path.Combine(GetSolutionPath(), @"YQ.Web\App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", data_path);

            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }

        protected string GetSolutionPath()
        {
            var regex = new Regex(@".*?(?=Smart\.Tests)", RegexOptions.IgnoreCase);
            return regex.Match(AppDomain.CurrentDomain.BaseDirectory).Value;
        }
    }
}