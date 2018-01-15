using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Smart.Core.Extensions;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Smart.Core.Export
{
    /// <summary>
    /// Excel 操作工具类
    /// </summary>
    public class NPOIExcelExport : ExportBase, IExport
    {
        public void Export(object data, string fileName, Model.ColumnModel colModel)
        {
            IWorkbook book = null;
            if (data is DataTable)
            {
                book = Export(data as DataTable, colModel);
            }
            else if (data is IEnumerable)
            {
                book = Export(data as IEnumerable, colModel);
            }
            else
            {
                throw new ArgumentException("data");
            }
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                book.Write(fs);
                fs.Flush();
            }
        }

        public Stream ToStream(object data, Model.ColumnModel colModel)
        {
            IWorkbook book = null;
            if (data is DataTable)
            {
                book = Export(data as DataTable, colModel);
            }
            else if (data is IEnumerable)
            {
                book = Export(data as DataTable, colModel);
            }
            else
            {
                throw new ArgumentException("data");
            }
            var ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private IWorkbook Export(DataTable data, Model.ColumnModel colModel)
        {
            colModel.ThrowIfNull("");
            var book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet(data.TableName ?? "Sheet1");

            #region 生成表头
            var dataColumns = base.ProcessHeader(colModel);
            CreateHeader(book, sheet, colModel);
            #endregion
            #region 生成数据行
            var rownum = colModel.Count;
            var cellstyles = GetColumnStyles(book, dataColumns);
            foreach (DataRow row in data.Rows)
            {
                IRow dataRow = sheet.CreateRow(rownum++);
                foreach (var col in dataColumns)
                {
                    var cell = dataRow.CreateCell(col.X);
                    cell.CellStyle = cellstyles[col.X];
                    cell.SetCellValue(row[col.Field].ToString());
                }
            }
            #endregion

            return book;
        }
        private IWorkbook Export(IEnumerable data, Model.ColumnModel colModel)
        {
            colModel.ThrowIfNull("");
            var book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            #region 生成表头
            var dataColumns = base.ProcessHeader(colModel);
            CreateHeader(book, sheet, colModel);
            #endregion
            #region 生成数据行
            var rownum = colModel.Count;
            var cellstyles = GetColumnStyles(book, dataColumns);
            PropertyInfo[] pis = null;
            foreach (var row in data)
            {
                if (pis == null) pis = row.GetType().GetProperties();
                IRow dataRow = sheet.CreateRow(rownum++);
                foreach (var col in dataColumns)
                {
                    var pi = pis.FirstOrDefault(p => p.Name == col.Field);
                    if (pi != null)
                    {
                        var cell = dataRow.CreateCell(col.X);
                        cell.CellStyle = cellstyles[col.X];
                        cell.SetCellValue(pi.GetValue(row, null).ToString());
                    }
                }
            }
            #endregion

            return book;
        }

        private void CreateHeader(IWorkbook book, ISheet sheet, Model.ColumnModel colModel)
        {
            var rownum = 0;
            IFont font = book.CreateFont();
            font.IsBold = true;
            foreach (var cols in colModel)
            {
                IRow headRow = sheet.CreateRow(rownum++);
                foreach (var col in cols)
                {
                    if (col.Width > 0) sheet.SetColumnWidth(col.X, col.Width * 36);
                    var cell = headRow.CreateCell(col.X);
                    var cellstyle = book.CreateCellStyle();
                    cell.CellStyle = cellstyle;
                    cellstyle.SetFont(font);
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    cellstyle.Alignment = HorizontalAlignment.Center;
                    cell.SetCellValue(col.Title);
                    if (col.ColSpan > 1 || col.RowSpan > 1)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(col.Y, col.Y + col.RowSpan - 1, col.X, col.X + col.ColSpan - 1));
                    }
                }
            }
        }

        private ICellStyle[] GetColumnStyles(IWorkbook book, Model.Column[] columns)
        {
            var cellstyles = new ICellStyle[columns.Length];
            foreach (var col in columns)
            {
                var cellstyle = book.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.Alignment = ((int)col.Alignment).As<HorizontalAlignment>();
                cellstyles[col.X] = cellstyle;
            }
            return cellstyles;
        }
    }

}
