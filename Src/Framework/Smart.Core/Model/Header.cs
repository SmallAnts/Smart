using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Smart.Core.Model
{
    /// <summary>
    /// 表头信息
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Header
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Row this[int index]
        {
            get { return Rows[index]; }
        }

        private List<Row> _rows;
        /// <summary>
        /// 
        /// </summary>
        public List<Row> Rows { get { return _rows ?? (_rows = new List<Row>()); } }

        /// <summary>
        /// 
        /// </summary>
        public int RowCount { get { return this.Rows.Count; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Row CreateRow()
        {
            var row = new Row();
            Rows.Add(row);
            return row;
        }
    }
}
