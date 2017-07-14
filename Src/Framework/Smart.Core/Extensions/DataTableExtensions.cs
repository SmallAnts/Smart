using System.Data;
using System.IO;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// 将DataTable导出Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="header"></param>
        public static void ExportExcel(this DataTable dt, string fileName, Export.Model.ColumnModel colModel)
        {
            var export = SmartContext.Current.Resolve<Export.IExport>();
            export.Export(dt, fileName, colModel);
        }

        /// <summary>
        /// 将DataTable导出Excel流
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="header"></param>
        public static Stream ExportExcel(this DataTable dt, Export.Model.ColumnModel colModel)
        {
            var export = SmartContext.Current.Resolve<Export.IExport>();
            return export.ToStream(dt, colModel);
        }
    }
}
