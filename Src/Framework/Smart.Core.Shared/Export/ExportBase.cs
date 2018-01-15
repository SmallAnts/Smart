using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.Core.Export
{
    public abstract class ExportBase
    {
        protected Model.Column[] ProcessHeader(Model.ColumnModel colModel)
        {
            #region 创建标记对象
            var colsize = colModel[0].Sum(e => e.ColSpan); // 总列数
            var maps = new int[colModel.Count, colsize];
            var columnNames = new Model.Column[colsize];
            #endregion

            for (int i = 0; i < colModel.Count; i++)
            {
                var colIndex = 0; // 当前列位置
                for (int j = 0; j < colModel[i].Length; j++)
                {
                    var column = colModel[i][j];
                    #region 计算行列位置
                    column.Y = i;  // 实际行号
                    if (i > 0)  // 从第2行开始纠正受rowspan影响的位置
                    {
                        for (var t = colIndex; t < colsize; t++)
                        {
                            if (maps[i, t] == 0) break;
                            colIndex++;
                        }
                    }
                    column.X = colIndex;
                    colIndex += column.ColSpan; // 下一列开始位置
                    #endregion

                    #region  标记数据绑定列

                    if (column.Y + 1 + column.RowSpan >= colModel.Count)
                    {
                        columnNames[column.X] = column;
                    }
                    #endregion

                    #region 标记被合并的单元格为 1

                    if (column.RowSpan > 1)
                    {
                        for (var r = 0; r < column.RowSpan; r++)
                        {
                            for (var c = 0; c < column.ColSpan; c++)
                            {
                                if (r > 0 || c > 0) maps[column.Y + r, column.X + c] = 1;
                            }
                        }
                    }
                    #endregion
                }
            }

            return columnNames;
        }
    }
}
