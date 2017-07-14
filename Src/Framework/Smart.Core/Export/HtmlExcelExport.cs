using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Smart.Core.Extensions;
using System.Reflection;

namespace Smart.Core.Export
{
    /*
     Style:
      文本：vnd.ms-excel.numberformat:@</para>
      日期：vnd.ms-excel.numberformat:yyyy/mm/dd</para>
      数字：vnd.ms-excel.numberformat:#,##0.00   </para>
      货币：vnd.ms-excel.numberformat:￥#,##0.00</para>
      百分比：vnd.ms-excel.numberformat: #0.00%</para>
         */
    public class HtmlExcelExport : ExportBase, IExport
    {
        public void Export(object data, string fileName, Model.ColumnModel colModel)
        {
            var table = string.Empty;
            if (data is DataTable)
            {
                table = Export(data as DataTable, colModel);
            }
            else if (data is IEnumerable)
            {
                table = Export(data as IEnumerable, colModel);
            }
            else
            {
                throw new ArgumentException("data");
            }
            using (var sr = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                sr.Write(table);
                sr.Flush();
            }
        }

        public Stream ToStream(object data, Model.ColumnModel colModel)
        {
            var table = string.Empty;
            if (data is DataTable)
            {
                table = Export(data as DataTable, colModel);
            }
            else if (data is IEnumerable)
            {
                table = Export(data as IEnumerable, colModel);
            }
            else
            {
                throw new ArgumentException("data");
            }
            var buffer = Encoding.UTF8.GetBytes(table);
            var ms = new MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private string Export(DataTable data, Model.ColumnModel colModel)
        {
            colModel.ThrowIfNull("");
            var table = new StringBuilder();
            table.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body>");
            table.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            #region 生成表头
            var dataColumns = base.ProcessHeader(colModel);
            CreateHeader(table, colModel);
            #endregion

            #region 生成数据行
            foreach (DataRow row in data.Rows)
            {
                table.Append("<tr>");
                foreach (var col in dataColumns)
                {
                    if (col.Alignment == Model.HorizontalAlignment.General)
                    {
                        table.AppendFormat("<td>{0}</td>", row[col.Field]);
                    }
                    else
                    {
                        table.AppendFormat("<td style=\"text-align:{1}\">{0}</td>",
                            row[col.Field], col.Alignment.ToString().ToLower());
                    }
                }
                table.Append("</tr>");
            }
            #endregion

            table.Append("</table></body></html>");
            return table.ToString();
        }
        private string Export(IEnumerable data, Model.ColumnModel colModel)
        {
            colModel.ThrowIfNull("");
            var table = new StringBuilder();
            table.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body>");
            table.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            #region 生成表头
            var dataColumns = base.ProcessHeader(colModel);
            CreateHeader(table, colModel);
            #endregion

            #region 生成数据行
            PropertyInfo[] pis = null;
            foreach (object row in data)
            {
                if (pis == null) pis = row.GetType().GetProperties();
                table.Append("<tr>");
                foreach (var col in dataColumns)
                {
                    var pi = pis.FirstOrDefault(p => p.Name == col.Field);
                    if (pi != null)
                    {
                        var value = pi.GetValue(row, null);
                        table.AppendFormat("<td style=\"vnd.ms-excel.numberformat:@;text-align:{1}\">{0}</td>", value, col.Alignment.ToString().ToLower());
                    }
                }
                table.Append("</tr>");
            }
            #endregion

            table.Append("</table></body></html>");
            return table.ToString();
        }

        private void CreateHeader(StringBuilder table, Model.ColumnModel colModel)
        {
            foreach (var row in colModel)
            {
                table.Append("<tr>");
                foreach (var col in row)
                {
                    table.AppendFormat("<th{1}{2}{3}>{0}</th>",
                        col.Title,
                        col.ColSpan > 1 ? $" colspan={col.ColSpan}" : string.Empty,
                        col.RowSpan > 1 ? $" rowspan={col.RowSpan}" : string.Empty,
                        col.Width > 0 ? $" width={col.Width}" : string.Empty
                    );
                }
                table.Append("</tr>");
            }
        }
    }
}
