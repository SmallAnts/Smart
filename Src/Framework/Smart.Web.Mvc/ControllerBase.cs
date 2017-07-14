using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Smart.Core.Extensions;
using System.IO;

namespace Smart.Web.Mvc
{
    /// <summary>
    /// 自定义控制器基础类
    /// </summary>
    public class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResultEx
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="writeData"></param>
        protected FileStreamResult Download(string filename, Action<HttpResponseBase> writeData)
        {
            return File(new FileStream(filename, FileMode.Open),
                "application/octet-stream",//"text/plain"
                Path.GetFileNameWithoutExtension(filename));
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.Buffer = false;
            //Response.ContentType = "application/octet-stream";
            //Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            //writeData(Response);
            //Response.Flush();
            //Response.End();
        }

        /// <summary>
        /// 上传文件, 示例：
        /// <para>按默认规则生成路径：this.Upload();</para>
        /// 自定义保存的文件名：this.Upload((i,f) => Path.Combine("/upload/user/", User.GetUserId()));
        /// </summary>
        /// <param name="setFileName">自定义文件名，返回网站相对路径，默认按GUID生成。</param>
        /// <param name="savedCallback"></param>
        /// <returns></returns>
        public UploadResult Upload(
            Func<int, string, string> setFileName = null,
            Action<HttpPostedFileBase, string> savedCallback = null)
        {
            try
            {
                List<string> files = new List<string>();
                var index = HttpContext.Request.QueryString["index"].AsInt();
                for (int i = 0; i < HttpContext.Request.Files.Count; i++)
                {
                    var file = HttpContext.Request.Files[i];
                    var fileName = string.Empty;
                    if (setFileName == null)
                        fileName = "/upload/" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    else
                        fileName = setFileName(index, file.FileName) + Path.GetExtension(file.FileName);
                    var strFullName = HttpContext.Server.MapPath(fileName);
                    if (System.IO.File.Exists(strFullName))
                    {
                        System.IO.File.Delete(strFullName);
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
                return new UploadResult { Error = "unknown Exception".T() };
#endif

            }
        }

        /// <summary>
        /// 导出Txt文件
        /// </summary>
        /// <param name="fileName">文件名，当不指定导出内容时文件名要求绝对路径</param>
        /// <param name="content">导出内容</param>
        public void ExportTxt(string fileName, string content = null)
        {
            Export(fileName, "application/octet-stream", content);
        }

        /// <summary>
        /// 导出Pdf文件
        /// </summary>
        /// <param name="fileName">文件名，当不指定导出内容时文件名要求绝对路径</param>
        public void ExportPdf(string fileName)
        {
            Export(fileName, "application/pdf");
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="fileName">文件名，当不指定导出内容时文件名要求绝对路径</param>
        /// <param name="contentType">导出内容类型</param>
        /// <param name="content">导出内容</param>
        public void Export(string fileName, string contentType, string content = null)
        {
            var response = HttpContext.Response;
            var DownloadFile = new FileInfo(fileName);
            response.Clear();
            response.ClearHeaders();
            response.Buffer = false;
            response.Charset = "utf-8";
            response.ContentType = contentType + ";charset=utf-8";
            response.HeaderEncoding = System.Text.Encoding.UTF8;
            response.ContentEncoding = System.Text.Encoding.UTF8;
            if (content == null)
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, Encoding.UTF8));
                //response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                response.TransmitFile(DownloadFile.FullName);
            }
            else
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\"></head><body>");
                response.Write(content);
                response.Write("</body></html>");
            }
            response.Flush();
            response.End();
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
