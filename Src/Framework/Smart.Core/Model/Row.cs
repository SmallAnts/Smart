using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Smart.Core.Model
{
    /// <summary>
    /// 行信息
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Row
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Column this[int index]
        {
            get { return Columns[index]; }
        }

        private List<Column> _columns;
        /// <summary>
        /// 
        /// </summary>
        public List<Column> Columns { get { return _columns ?? (_columns = new List<Column>()); } }


        /// <summary>
        /// 
        /// </summary>
        public int ColumnCount { get { return this.Columns.Count; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Column CreateColumn(string title, string field = null)
        {
            var column = new Column() { Field = field, Title = title };
            this.Columns.Add(column);
            return column;
        }
    }
}
