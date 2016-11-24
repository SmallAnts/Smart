using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Smart.Core.Model
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Row
    {
        private List<Column> _columns;
        public List<Column> Columns { get { return _columns ?? (_columns = new List<Column>()); } }

        public Column CreateColumn(string text)
        {
            var column = new Column() { Text = text };
            this.Columns.Add(column);
            return column;
        }
    }
}
