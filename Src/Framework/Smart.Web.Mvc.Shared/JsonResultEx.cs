using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Mvc;

namespace Smart.Web.Mvc
{
    /// <summary>
    /// JsonResult 扩展 忽略循环引用
    /// </summary>
    public class JsonResultEx : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonResultEx()
        {
            Settings = new JsonSerializerSettings
            {
                // 忽略循环引用.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("不允许 JSON GET");
            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data == null)
            {
                return;
            }
            var scriptSerializer = JsonSerializer.Create(this.Settings);
            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}