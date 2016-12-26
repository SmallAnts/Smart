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
        public void Export()
        {
            var book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            var sheet = book.CreateSheet("Sheet1");

            var row = sheet.CreateRow(0);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 5));
            row.CreateCell(0).SetCellValue("列1");
            row.CreateCell(6).SetCellValue("列2");
            var row2 = sheet.CreateRow(1);
            row2.CreateCell(0).SetCellValue("列3");

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls");
            using (var file = new FileStream(path, FileMode.Create))
            {
                book.Write(file);
                file.Close();
            }
        }

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
            var header = Core.Utilites.ExcelUtility.CreateHeader();
            var row = header.CreateRow();
            row.CreateColumn("姓名", "Name").SetAlignment(Core.Model.HorizontalAlignment.Center);
            row.CreateColumn("性别", "Sex");
            row.CreateColumn("出生日期", "Brithday").SetDataFromat("yyyy-MM-dd").SetWidth(12);
            row.CreateColumn("年龄", "Age");

            var sheetName = "List导出Excel测试";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "list.xls");
            list.ExportExcel(path, sheetName, header);
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
            var header = Core.Utilites.ExcelUtility.CreateHeader();
            var row = header.CreateRow();
            row.CreateColumn("姓名", "Name").SetAlignment(Core.Model.HorizontalAlignment.Center);
            row.CreateColumn("性别", "Sex");
            row.CreateColumn("出生日期", "Brithday").SetDataFromat("yyyy-MM-dd").SetWidth(12);
            row.CreateColumn("年龄", "Age");

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
