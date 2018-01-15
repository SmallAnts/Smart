using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.AdminLte
{
    public class AdminLteHtmlHelper
    {
        public MvcHtmlString SidebarMenu(List<UI.Menu> menus)
        {
            var html = new StringBuilder();
            html.Append("<ul class=\"sidebar-menu\">");
            CreateSidebarMenu(html, menus);
            html.Append("</ul>");
            return MvcHtmlString.Create(html.ToString());
        }
        private void CreateSidebarMenu(StringBuilder html, List<UI.Menu> menus)
        {
            foreach (var item in menus)
            {
                string url = null;
                string icon1 = null;
                string icon2 = null;
                var attrs = new StringBuilder();
                if (!item.Icon.IsEmpty()) icon1 = string.Format("<i class=\"fa {0}\"></i>", item.Icon);
                if (item.Menus.Any()) icon2 = "<i class=\"fa pull-right fa-angle-left\"></i>";
                //else if (!item.SecondIcon.IsEmpty()) icon2 = string.Format("<i class=\"fa pull-right {0}\"></i>", item.SecondIcon);
                if (item.HtmlAttributes != null)
                {
                    foreach (var p in item.HtmlAttributes.GetType().GetProperties())
                    {
                        attrs.AppendFormat(" {0}=\"{1}\"", p.Name.StartsWith("data_") ? p.Name.Replace("_", "_") : p.Name, p.GetValue(item.HtmlAttributes));
                    }
                }
                if (item.Url.IsEmpty())
                {
                    html.AppendFormat("<li{0}>{1}<span>{2}</span>{3}", attrs.ToString(), icon1, item.Text, icon2);
                }
                else
                {
                    html.AppendFormat("<li{0}><a href=\"{1}\"{2}>{3}<span>{4}</span>{5}</a>",
                        item.Menus.Any() ? " class=\"treeview\"" : string.Empty, item.Url, attrs.ToString(), icon1, item.Text, icon2);
                }
                if (item.Menus.Any())
                {
                    html.AppendFormat("<ul class=\"treeview-menu\">", url, icon1, item.Text, icon2);
                    CreateSidebarMenu(html, item.Menus);
                    html.AppendFormat("</ul>", url, icon1, item.Text, icon2);
                }
                html.Append("</li>");
            }
        }

    }
}
