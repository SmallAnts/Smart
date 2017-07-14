using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Smart.Core.Export
{
    /// <summary>
    /// 数据导出接口
    /// </summary>
    public interface IExport
    {
        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="colModel"></param>
        void Export(object data, string fileName, Model.ColumnModel colModel);

        /// <summary>
        /// 导出到流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colModel"></param>
        /// <returns></returns>
        Stream ToStream(object data, Model.ColumnModel colModel);
    }
}
