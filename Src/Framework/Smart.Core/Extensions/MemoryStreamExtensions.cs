using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemoryStreamExtensions
    {
        public static void Save(this MemoryStream ms, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }
    }
}
