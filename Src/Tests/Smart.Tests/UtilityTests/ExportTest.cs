using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class ExportTest : TestBase
    {
        public ExportTest() : base() { }

        [TestMethod]
        public void ListExport()
        {
            var list = new List<UserInfo>();
            for (int i = 0; i < 10; i++)
            {
                var birthday = new DateTime(1985 + i, i + 1, i + 1);
                list.Add(new UserInfo { Name = "姓名" + i, Sex = i % 2 == 0 ? "女" : "男", Birthday = birthday, Age = birthday.GetAge().Year });
            }
            // 创建表头
            var header = Core.Utilites.ExportUtility.CreateHeader();
            var row = header.CreateRow();
            row.CreateColumn("姓名").SetAlignment(Core.Model.HorizontalAlignment.Center);
            row.CreateColumn("性别");
            row.CreateColumn("出生日期").SetDataFromat("yyyy-MM-dd").SetWidth(12);
            row.CreateColumn("年龄");

            var sheetName = "List导出Excel测试";
            list.ExportExcel(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "list.xls"), sheetName, header);
        }

        [TestMethod]
        public void DatatableExport()
        {
            // 创建导出数据
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Sex");
            dt.Columns.Add("Birthday", typeof(DateTime));
            dt.Columns.Add("Age", typeof(int));
            for (int i = 0; i < 10; i++)
            {
                var birthday = new DateTime(1985 + i, i + 1, i + 1);
                dt.Rows.Add("姓名" + i, i % 2 == 0 ? "女" : "男", birthday, birthday.GetAge().Year);
            }

            // 创建表头
            var header = Core.Utilites.ExportUtility.CreateHeader();
            var row = header.CreateRow();
            row.CreateColumn("姓名").SetAlignment(Core.Model.HorizontalAlignment.Center);
            row.CreateColumn("性别");
            row.CreateColumn("出生日期").SetDataFromat("yyyy-MM-dd").SetWidth(12);
            row.CreateColumn("年龄");

            var sheetName = "DataTable导出Excel测试";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datatable.xls");

            dt.ExportExcel(path, sheetName, header);
        }

        class UserInfo
        {
            public string Name { get; set; }
            public string Sex { get; set; }
            public DateTime Birthday { get; set; }
            public int Age { get; set; }

        }
    }
}
