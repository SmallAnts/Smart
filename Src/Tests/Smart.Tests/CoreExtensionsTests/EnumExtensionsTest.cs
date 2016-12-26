using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class EnumExtensionsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var enum1 = TestEnum.One;
            var has = enum1.HasFlag(TestEnum.Two);
            enum1 = enum1.DelFlag(TestEnum.One);
            enum1 = enum1.SetFlag(TestEnum.Two | TestEnum.Two | TestEnum.Three);
            //enum1.Add(TestEnums.Three);

            var enum12 = TestEnums.One | TestEnums.Two;
            has = enum12.HasFlag(TestEnums.One);
            //has = enum12.HasFlag(TestEnums.Three);
            enum12 = enum12.SetFlag(TestEnums.Three);
            enum12 = enum12.DelFlag(TestEnums.Two | TestEnums.Three);
        }


        public enum TestEnum
        {
            One = 1,
            Two = 2,
            Three = 4,
        }

        [Flags]
        public enum TestEnums
        {
            One = 1,
            Two = 2,
            Three = 4
        }
    }


}
