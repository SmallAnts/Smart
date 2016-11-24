using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Model
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Header
    {
        private List<Row> _rows;
        public List<Row> Rows { get { return _rows ?? (_rows = new List<Row>()); } }

        public Row CreateRow()
        {
            var row = new Row();
            Rows.Add(row);
            return row;
        }
    }
}
