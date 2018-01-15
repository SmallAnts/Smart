using System.IO;
using System.Security.Permissions;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// 获取文件的 Mime Type 名称
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMimeType(this FileInfo file)
        {
            var regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\HKEY_CLASSES_ROOT");
            var classesRoot = Microsoft.Win32.Registry.ClassesRoot;
            var extension = file.Extension.ToUpper();
            var typeKey = classesRoot.OpenSubKey(@"MIME\Database\Content Type");

            foreach (string keyname in typeKey.GetSubKeyNames())
            {
                var currentKey = classesRoot.OpenSubKey(@"MIME\Database\Content Type\" + keyname);
                string currentExtension = (string)currentKey.GetValue("Extension", null);
                if (!string.IsNullOrEmpty(currentExtension) && currentExtension.ToUpper() == extension)
                    return keyname;
            }
            return string.Empty;
        }
    }
}
