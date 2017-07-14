using System.ComponentModel;
using System.Drawing;
using Smart.Core.Extensions;
using System;

namespace Smart.Core.Export.Model
{
    /// <summary>
    /// 列信息
    /// </summary>
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    public class Column
    {
        /// <summary>
        /// 标题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 对应数据源中的列名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 宽度（单位像素，不同的插件可能会有偏差）
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 水平对齐方式
        /// </summary>
        public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.General;

        private int _rowspan = 1;
        /// <summary>
        /// 占行数
        /// </summary>
        public int RowSpan
        {
            get { return _rowspan; }
            set { _rowspan = Math.Max(1, value); }
        }

        private int _colspan = 1;
        /// <summary>
        /// 占列数
        /// </summary>
        public int ColSpan
        {
            get { return _colspan; }
            set { _colspan = Math.Max(1, value); }
        }

        /// <summary>
        /// 实际列位置，从0开始
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        /// 实际行位置，从0开始
        /// </summary>
        public int Y { get; internal set; }
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
        //Fill = 4,
        //
        Justify = 5,
        //
        //CenterSelection = 6,
        //
        //Distributed = 7
    }
}
