using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Smart.Tests
{
    [TestClass]
    public class TypeFinderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Core.Infrastructure.ITypeFinder typeFinder = new Core.Infrastructure.DirectoryTypeFinder();
            var t1 = typeFinder.ForTypesDerivedFrom<Core.Infrastructure.ITypeFinder>().ToArray();
        }
    }
}
