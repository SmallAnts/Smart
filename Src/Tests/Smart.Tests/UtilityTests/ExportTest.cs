using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Smart.Core.Dependency;
using Autofac;
using Smart.Core.Configuration;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class ExportTest : TestBase
    {
        public ExportTest() : base()
        {

        }

        [TestMethod]
        public void ListExportTest()
        {
            var list = new List<UserInfo>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new UserInfo { Name = "XM" + i.ToString(), Sex = (i % 2).ToString(), Birthday = DateTime.Today, Age = i });
            }
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls");
            var colModel = CreateColumnModel();
            list.ExportExcel(filename, colModel);
        }

        [TestMethod]
        public void DataTableExportTest()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Sex");
            dataTable.Columns.Add("Birthday");
            dataTable.Columns.Add("Age");
            for (int i = 0; i < 10; i++)
            {
                dataTable.Rows.Add(i.ToString(), i % 2, DateTime.Today, i);
            }

            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls");
            var colModel = CreateColumnModel();
            dataTable.ExportExcel(filename, colModel);
        }

        private Smart.Core.Export.Model.ColumnModel CreateColumnModel()
        {
            var colModel = new Core.Export.Model.ColumnModel() {
                new Core.Export.Model.Column[] {
                    new Core.Export.Model.Column { Field="Name",Title="Name", RowSpan=2, Alignment= Core.Export.Model.HorizontalAlignment.Center  },
                    new Core.Export.Model.Column {Title="基本信息",ColSpan=3 },
                },
                new Core.Export.Model.Column[] {
                    new Core.Export.Model.Column { Field="Sex",Title="Sex" },
                    new Core.Export.Model.Column { Field="Birthday", Title="Birthday" ,Width=150},
                    new Core.Export.Model.Column { Field="Age", Title="Age", Alignment= Core.Export.Model.HorizontalAlignment.Right },
                },
            };

            return colModel;
        }


        [TestMethod]
        public void TableTest()
        {
            #region 测试数据，表头二维数组
            var data = new Cell[][] {
                new Cell[] {
                    new Cell {RowSpan=3,Field="A" },  new Cell { RowSpan=2, ColSpan=2, Field="B" }, new Cell {RowSpan=3,Field="C" },  new Cell { ColSpan=3,Field="D" },
                },
                new Cell[] {
                    new Cell { ColSpan=2,Field="E" },  new Cell { ColSpan=2,Field="F" }
                },
                new Cell[] {
                    new Cell() { Field="G"},  new Cell() { Field="H"} ,new Cell() { Field="I"},  new Cell() { Field="J"},
                }
            };
            #endregion

            #region 创建标记对象
            var colsize = data[0].Sum(e => e.ColSpan); // 总列数
            var maps = new int[data.Length, colsize];
            var columnNames = new string[colsize];
            #endregion

            for (int i = 0; i < data.Length; i++)
            {
                var colIndex = 0; // 当前列位置
                for (int j = 0; j < data[i].Length; j++)
                {
                    var cell = data[i][j];
                    #region 计算行列位置
                    cell.Y = i;  // 实际行号
                    if (i > 0)  // 从第2行开始纠正受rowspan影响的位置
                    {
                        for (var t = colIndex; t < colsize; t++)
                        {
                            if (maps[i, t] == 0) break;
                            colIndex++;
                        }
                    }
                    cell.X = colIndex;
                    colIndex += cell.ColSpan; // 下一列开始位置
                    #endregion

                    #region  标记数据绑定列

                    if (cell.Y + 1 + cell.RowSpan >= data.Length)
                    {
                        columnNames[cell.X] = cell.Field;
                    }
                    #endregion

                    #region 标记被合并的单元格为 1

                    if (cell.RowSpan > 1)
                    {
                        for (var r = 0; r < cell.RowSpan; r++)
                        {
                            for (var c = 0; c < cell.ColSpan; c++)
                            {
                                if (r > 0 || c > 0) maps[cell.Y + r, cell.X + c] = 1;
                            }
                        }
                    }
                    #endregion
                }
            }

        }
        public class Cell
        {
            public string Field { get; set; }
            public string Title { get; set; }
            public int RowSpan { get; set; } = 1;
            public int ColSpan { get; set; } = 1;

            internal int X { get; set; }
            internal int Y { get; set; }
        }


        class UserInfo
        {
            public string Name { get; set; }
            public string Sex { get; set; }
            public DateTime Birthday { get; set; }
            public int Age { get; set; }

        }



    }

    // 依赖注册
    class RegistrarIExport : IDependencyRegistrar
    {
        public int Order { get { return 0; } }

        public void Register(ContainerBuilder builder, SmartConfig config)
        {
            builder.RegisterType<Core.Export.NPOIExcelExport>().As<Core.Export.IExport>();
        }
    }
}
