using Smart.Web.Mvc.UI.JqGrid;
using System.Web.Mvc;

namespace Smart.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static Grid JqGrid(this HtmlHelper helper, string id, string caption = null, string url = null, bool asyncLoad = false)
        {
            return new Grid(id, caption, url, asyncLoad);
        }
    }
}
