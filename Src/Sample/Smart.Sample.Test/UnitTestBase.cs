using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;

namespace Smart.Sample.Test
{
    [TestClass]
    public class UnitTestBase
    {
        public UnitTestBase()
        {
            var data_path = Path.Combine(GetSolutionPath(), @"Smart.Sample.Web\App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", data_path);
        }

        protected string GetSolutionPath()
        {
            var regex = new Regex(".*?(?=Smart.Sample.Test)", RegexOptions.IgnoreCase);
            return regex.Match(AppDomain.CurrentDomain.BaseDirectory).Value;
        }


        public void Test()
        {
            var t = new object();
        }
    }
}
