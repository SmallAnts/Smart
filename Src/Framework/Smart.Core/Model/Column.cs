using NPOI.SS.UserModel;
using System.ComponentModel;
using System.Drawing;
using Smart.Core.Extensions;
using System;

namespace Smart.Core.Model
{
    /// <summary>
    /// 列信息
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Column
    {
        /// <summary>
        /// 水平对齐方式
        /// </summary>
        private HorizontalAlignment _alignment;
        /// <summary>
        /// 合并的列数
        /// </summary>
        private int _colSpan = 1;
        /// <summary>
        /// 格式化字符串
        /// </summary>
        private string _dataFormat;
        /// <summary>
        /// 字体
        /// </summary>
        private Font _font;
        /// <summary>
        /// 合并的行数
        /// </summary>
        private int _rowSpan = 1;
        /// <summary>
        /// 宽度
        /// </summary>
        private int _width;

        public string Title { get; set; }
        public string Field { get; set; }

        public Column SetWidth(int width)
        {
            _width = width;
            return this;
        }
        public int GetWidth()
        {
            return _width;
        }

        public Column SetDataFromat(string dataFormat)
        {
            _dataFormat = dataFormat;
            return this;
        }
        public string GetDataFormat()
        {
            return _dataFormat;
        }

        public Column SetAlignment(HorizontalAlignment alignment)
        {
            _alignment = alignment;
            return this;
        }
        public HorizontalAlignment GetAlignment()
        {
            return _alignment;
        }

        public Column SetFont(Font font)
        {
            _font = font;
            return this;
        }
        public Font GetFont()
        {
            return _font;
        }

        public Column SetRowSpan(int rowSpan)
        {
            _rowSpan = rowSpan;
            return this;
        }
        public int GetRowSpan()
        {
            return _rowSpan;
        }

        public Column SetColSpan(int colSpan)
        {
            _colSpan = colSpan;
            return this;
        }
        public int GetColSpan()
        {
            return _colSpan;
        }

        internal void ApplyHeaderCell(ICell cell)
        {
            var workbook = cell.Sheet.Workbook;
            var cellstyle = workbook.CreateCellStyle();
            cell.CellStyle = cellstyle;
            cell.SetCellValue(Field);
            if (_width > 0) cell.Sheet.SetColumnWidth(cell.ColumnIndex, _width * 256);
            if (_font != null)
            {
                IFont font = workbook.CreateFont();
                font.FontName = _font.Name;
                if (_font.Bold) font.Boldweight = 700;
                cellstyle.SetFont(font);
            }
            var align = (int)this._alignment;
            cellstyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)align;

        }
        internal void ApplyDataCell(ICell cell)
        {
            var workbook = cell.Sheet.Workbook;
            var cellstyle = workbook.CreateCellStyle();
            cell.CellStyle = cellstyle;
            if (!_dataFormat.IsEmpty())
            {
                var format = workbook.CreateDataFormat();
                cellstyle.DataFormat = format.GetFormat(this._dataFormat);
            }
            var align = (int)this._alignment;
            cellstyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)align;
        }
    }

    /// <summary>
    /// 水平对齐方式
    /// </summary>
    public enum HorizontalAlignment
    {
        //
        General = 0,
        //
        Left = 1,
        //
        Center = 2,
        //
        Right = 3,
        //
        Fill = 4,
        //
        Justify = 5,
        //
        CenterSelection = 6,
        //
        Distributed = 7
    }
}
