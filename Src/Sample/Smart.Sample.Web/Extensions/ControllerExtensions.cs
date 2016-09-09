using Smart.Core.Extensions;
using Smart.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YQ.Cashier.Domain.Entites;

namespace YQ.Cashier.Web
{
    public static class ControllerExtensions
    {
        public static T GetService<T>(this Controller controller, string name = null) where T : class
        {
            return Smart.Core.SmartContext.Current.Resolve<T>(name);
        }

        /// <summary>
        /// 上传文件, 示例：
        /// <para>按默认规则生成路径：this.Upload();</para>
        /// 自定义保存的文件名：this.Upload((i,f) => Path.Combine("/upload/user/", User.GetUserId()));
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="setFileName">自定义文件名，返回网站相对路径，默认按GUID生成。</param>
        /// <returns></returns>
        public static UploadResult Upload(this System.Web.Mvc.Controller controller,
            Func<int, string, string> setFileName = null,
            Action<HttpPostedFileBase, string> savedCallback = null)
        {
            try
            {
                var context = controller.HttpContext;
                List<string> files = new List<string>();
                var index = controller.Request.QueryString["index"].AsInt();
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];
                    var fileName = string.Empty;
                    if (setFileName == null)
                        fileName = "/upload/" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    else
                        fileName = setFileName(index, file.FileName) + Path.GetExtension(file.FileName);
                    var strFullName = context.Server.MapPath(fileName);
                    if (File.Exists(strFullName))
                    {
                        File.Delete(strFullName);
                    }
                    var strPath = Path.GetDirectoryName(strFullName);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    file.SaveAs(strFullName);
                    files.Add(fileName);
                    if (savedCallback != null)
                    {
                        savedCallback(file, fileName);
                    }
                }

                return new UploadResult { FileNames = files.ToArray(), Error = "" };
            }
            catch (Exception ex)
            {
#if DEBUG
                return new UploadResult { Error = ex.Message };
#else
                return new UploadResult { Error =  LangHelper.Get(l => l.unknownException) };
#endif

            }
        }
        /// <summary>
        /// 为JqGrid的editOption生成id:value数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="idProperty"></param>
        /// <param name="valueProperty"></param>
        /// <returns></returns>
        public static string JqGridOptions<T>(IList<T> list, string idProperty, string valueProperty)
        {
            if (list == null || list.Count == 0 || idProperty.IsEmpty() || valueProperty.IsEmpty()) return string.Empty;
            StringBuilder ret = null;
            for (int i = 0; i < list.Count; i++)
            {
                var id = list[i].GetType().GetProperty(idProperty).GetValue(list[i], null);
                var val = list[i].GetType().GetProperty(valueProperty).GetValue(list[i], null);
                if (i < list.Count - 1)
                    ret.Append(id + ":" + val + ";");
                else
                    ret.Append(id + ":" + val);
            }
            return ret.ToString();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

    }

    /// <summary>
    /// 上传返回结果对象
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// 上传后的文件名列表，相对站点根目录
        /// </summary>
        public string[] FileNames { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 其它返回结果
        /// </summary>
        public object Result { get; set; }
    }
}