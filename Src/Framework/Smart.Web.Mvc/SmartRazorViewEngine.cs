using Smart.Core;
using Smart.Core.Configuration;
using System.Web.Mvc;

namespace Smart.Web.Mvc
{
    /// <summary>
    /// Razor 视图引擎，支持主题，主题名称在 SmartConfig中配置
    /// </summary>
    public class SmartRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SmartRazorViewEngine()
        {
            var config = SmartContext.Current.Resolve<SmartConfig>();
            SetTheme(config.Theme);
        }

        public void SetTheme(string theme)
        {
            base.AreaViewLocationFormats = new string[]
            {
                "~/Areas/Themes/"+theme+"/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/Themes/"+theme+"/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };

            base.AreaMasterLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };

            base.AreaPartialViewLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };

            base.ViewLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };

            base.MasterLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };

            base.PartialViewLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };
            base.FileExtensions = new string[]
            {
                "cshtml",
            };
        }
    }
}
