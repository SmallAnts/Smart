using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class EnumExtensionsTest
    {
        [Display(Name = "测试")]
        public int Test { get; set; }

        public TestEnum? te;
        [TestMethod]
        public void TestMethod1()
        {
            object abc = "123";
            // {Value:int ,Text:string}
            var list = new TestEnum().ToValueNameList();
            // {Value:Enum ,Text:string}
            var list2 = new TestEnum().ToValueNameList(intValue: false);

            var list3 = te.ToValueNameList();

            string e1 = te.GetDisplayName();
            te = TestEnum.Two;
            e1 = te.GetDisplayName();

            string e2 = new EnumExtensionsTest().GetDisplayName(t => t.Test);
            var enum1 = TestEnum.One;
            var text = enum1.GetDisplayName();
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
            [Display(Name = "一")]
            One = 1,
            [Display(Name = "二")]
            Two = 2,
            [Display(Name = "三")]
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
