using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void TestDeserializeObject()
        {
            var json = "{\"test.001\":\"test\"}".JsonTo<dynamic>();
            var test = json["test.001"];
            var test2 = json["test.002"];
            Assert.AreEqual(test.ToString(), "test");
        }

        [TestMethod]
        public void Test()
        {
            var colums = new Core.Export.Model.Column().GetPropertyNames(t => new { t.RowSpan, t.ColSpan });
        }

    }
}
