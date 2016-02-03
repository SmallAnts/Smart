using Smart.Core.Localization;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Smart.Web.Mvc.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebPageBaseExtensions
    {
        public static MvcHtmlString T(this WebPageBase webpage, string key, params object[] args)
        {
            var value = Language.Get(key, args);
            return new MvcHtmlString(value);
        }
    }
}