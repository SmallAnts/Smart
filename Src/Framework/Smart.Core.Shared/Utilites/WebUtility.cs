using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Smart.Core.Utilites
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public class WebUtility
    {
        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            var req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            var reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            url = BuildGetUrl(url, parameters);
            var req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        ///// <summary>
        ///// 执行带文件上传的HTTP POST请求。
        ///// </summary>
        ///// <param name="url">请求地址</param>
        ///// <param name="textParams">请求文本参数</param>
        ///// <param name="fileParams">请求文件参数</param>
        ///// <returns>HTTP响应</returns>
        //public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams)
        //{
        //    // 如果没有文件参数，则走普通POST请求
        //    if (fileParams == null || fileParams.Count == 0)
        //    {
        //        return DoPost(url, textParams);
        //    }

        //    string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线

        //    HttpWebRequest req = GetWebRequest(url, "POST");
        //    req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

        //    System.IO.Stream reqStream = req.GetRequestStream();
        //    byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
        //    byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

        //    // 组装文本请求参数
        //    string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
        //    IEnumerator<KeyValuePair<string, string>> textEnum = textParams.GetEnumerator();
        //    while (textEnum.MoveNext())
        //    {
        //        string textEntry = string.Format(textTemplate, textEnum.Current.Key, textEnum.Current.Value);
        //        byte[] itemBytes = Encoding.UTF8.GetBytes(textEntry);
        //        reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        //        reqStream.Write(itemBytes, 0, itemBytes.Length);
        //    }

        //    // 组装文件请求参数
        //    string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
        //    IEnumerator<KeyValuePair<string, FileItem>> fileEnum = fileParams.GetEnumerator();
        //    while (fileEnum.MoveNext())
        //    {
        //        string key = fileEnum.Current.Key;
        //        FileItem fileItem = fileEnum.Current.Value;
        //        string fileEntry = string.Format(fileTemplate, key, fileItem.GetFileName(), fileItem.GetMimeType());
        //        byte[] itemBytes = Encoding.UTF8.GetBytes(fileEntry);
        //        reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        //        reqStream.Write(itemBytes, 0, itemBytes.Length);

        //        byte[] fileBytes = fileItem.GetContent();
        //        reqStream.Write(fileBytes, 0, fileBytes.Length);
        //    }

        //    reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
        //    reqStream.Close();

        //    HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        //    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        //    return GetResponseAsString(rsp, encoding);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开
            return true;
        }
        /// <summary>
        /// 初始化为指定的URI方案的一个新的system.net.webrequest实例。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public virtual HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Collect4Net";
            req.Timeout = this._timeout;

            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public virtual string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            using (rsp)
            {
                // 以字符流的方式读取HTTP响应
                using (var stream = rsp.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        public string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                url = url + (url.Contains("?") ? "&" : "?") + BuildQuery(parameters);
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam) postData.Append("&");

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}
