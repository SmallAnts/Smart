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
            var json = "{\"test.001\":\"test\"}".DeserializeObject<dynamic>();
            var test = json["test.001"];
            var test2 = json["test.002"];
            Assert.AreEqual(test.ToString(), "test");
        }
    }
}
