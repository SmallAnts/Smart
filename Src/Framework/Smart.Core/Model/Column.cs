using NPOI.SS.UserModel;
using System.ComponentModel;
using System.Drawing;
using Smart.Core.Extensions;
using System;

namespace Smart.Core.Model
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Column
    {
        private int _width;
        private string _dataFormat;
        private HorizontalAlignment _alignment;
        private Font _font;

        public string Text { get; set; }

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

        internal void ApplyHeaderCell(ICell cell)
        {
            var workbook = cell.Sheet.Workbook;
            var cellstyle = workbook.CreateCellStyle();
            cell.CellStyle = cellstyle;
            cell.SetCellValue(Text);
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
