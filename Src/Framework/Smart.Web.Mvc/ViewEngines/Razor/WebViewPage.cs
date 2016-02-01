using System;
using System.IO;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Smart.Web.Mvc.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public override void InitHelpers()
        {
            base.InitHelpers(); System.Web.Mvc.WebViewPage 
        }

        public MvcHtmlString T(string key, params object[] args)
        {
            return new MvcHtmlString(Core.Localization.Language.Get(key, args));
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}