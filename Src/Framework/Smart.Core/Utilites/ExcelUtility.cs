using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Smart.Core.Extensions;
using Smart.Core.Model;
using NPOI.SS.Util;

namespace Smart.Core.Utilites
{
    /// <summary>
    /// Excel 操作工具类
    /// </summary>
    public class ExcelUtility
    {
        /// <summary>
        /// 创建一个表头配置
        /// </summary>
        /// <returns></returns>
        public static Header CreateHeader()
        {
            return new Model.Header();
        }

        /// <summary>
        /// 将List导出Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="header"></param>
        public static void Export<T>(List<T> list, string fileName, string sheetName, Model.Header header = null)
        {
            if (!string.IsNullOrEmpty(fileName) && null != list && list.Count > 0)
            {
                var book = new HSSFWorkbook();
                ISheet sheet = book.CreateSheet(sheetName);
                var pis = typeof(T).GetProperties();
                // 生成表头
                if (header == null)
                {
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < pis.Length; i++)
                    {
                        row.CreateCell(i).SetCellValue(pis[i].Name);
                    }
                }
                else
                {
                    foreach (var item in header.Rows)
                    {
                        IRow row = sheet.CreateRow(0);
                        for (int i = 0; i < item.Columns.Count; i++)
                        {
                            item.Columns[i].ApplyHeaderCell(row.CreateCell(i));
                        }
                    }
                }

                // 生成数据行
                var headerRows = header == null ? 1 : header.Rows.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + headerRows);
                    for (int j = 0; j < pis.Length; j++)
                    {
                        //if (header != null && j >= header.ColumnCount) break;
                        var cell = row2.CreateCell(j);
                        SetCellValue(cell, pis[j].GetValue(list[i], null));
                        if (header != null)
                        {
                            var headercell = header.Rows[header.Rows.Count - 1].Columns[j];
                            headercell.ApplyDataCell(cell);
                        }
                    }
                }

                // 写入到文件  
                Save(book, fileName);
            }
        }

        /// <summary>
        /// 将DataTable导出Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void Export(DataTable dt, string fileName, string sheetName = null, Model.Header header = null)
        {
            if (!string.IsNullOrEmpty(fileName) && null != dt && dt.Rows.Count > 0)
            {
                var book = new HSSFWorkbook();
                ISheet sheet = book.CreateSheet(sheetName ?? dt.TableName);

                // 生成表头
                if (header == null)
                {
                    IRow row = sheet.CreateRow(0);
                    foreach (DataColumn column in dt.Columns)
                    {
                        sheet.SetColumnWidth(column.Ordinal, 10 * 256);
                        var cell = row.CreateCell(column.Ordinal);
                        cell.SetCellValue(column.ColumnName);
                    }
                }
                else
                {
                    foreach (var item in header.Rows)
                    {
                        IRow row = sheet.CreateRow(0);
                        for (int i = 0; i < item.Columns.Count; i++)
                        {
                            item.Columns[i].ApplyHeaderCell(row.CreateCell(i));
                        }
                    }
                }

                // 生成数据行
                var headerRows = header == null ? 1 : header.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + headerRows);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //if (header != null && j >= header.ColumnCount) break;
                        var cell = row2.CreateCell(j);
                        SetCellValue(cell, dt.Rows[i][j]);
                        if (header != null)
                        {
                            var headercell = header.Rows[header.Rows.Count - 1].Columns[j];
                            headercell.ApplyDataCell(cell);
                        }
                    }
                }

                // 写入到文件  
                Save(book, fileName);
            }
        }

        private static void CreateHeader(ISheet sheet, Model.Header header)
        {
            for (int i = 0; i < header.RowCount; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < header[i].ColumnCount; j++)
                {
                    ICell cell = row.CreateCell(j);
                    header[i][j].ApplyHeaderCell(cell);
             
                    if (header[i][j].GetRowSpan() > 1 || header[i][j].GetColSpan() > 1)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(i, i + header[i][j].GetRowSpan() - 1, j, j + header[i][j].GetColSpan() - 1));    // 合并单元格
                    }
                }
            }
        }
        private static void Save(IWorkbook wrokbook, string fileName)
        {
            // 写入到客户端  
            using (var ms = new MemoryStream())
            {
                wrokbook.Write(ms);
                ms.Save(fileName);
                wrokbook = null;
            }
        }
        private static void SetCellValue(ICell cell, object value)
        {
            var type = value.GetType().Name;
            switch (type)
            {
                case "DateTime":
                    cell.SetCellValue((DateTime)value);
                    break;
                case "Boolean":
                    cell.SetCellValue((bool)value);
                    break;
                case "Double":
                    cell.SetCellValue((Double)value);
                    break;
                default:
                    cell.SetCellValue(value.AsString());
                    break;
            }

        }
    }
}
