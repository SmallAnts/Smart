using System.Data;

namespace Smart.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static void ExportExcel(this DataTable dt, string fileName, string sheetName = null, Model.Header header = null)
        {
            Utilites.ExportUtility.ExportExcel(dt, fileName, sheetName, header);
        }
    }
}
