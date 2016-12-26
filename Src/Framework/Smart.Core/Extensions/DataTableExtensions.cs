using System.Data;

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
        /// <param name="sheetName"></param>
        /// <param name="header"></param>
        public static void ExportExcel(this DataTable dt, string fileName, string sheetName = null, Model.Header header = null)
        {
            Utilites.ExcelUtility.Export(dt, fileName, sheetName, header);
        }
    }
}
