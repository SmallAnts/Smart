using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Smart.Core.Extensions;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace Smart.Web.Mvc.UI.Ace
{
    public class AceHtmlHelper
    {
        public Navbar BeginNavbar(HtmlHelper htmlHelper, string id = null, object htmlAttributes = null)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.AddCssClass("navbar navbar-default");
            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            tagBuilder.GenerateId("navbar");
            var navbar = new Navbar(htmlHelper.ViewContext);
            return navbar;
        }

        public MvcHtmlString Navbar()
        {
            var html = new StringBuilder();
            return MvcHtmlString.Create(html.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iconClass"></param>
        /// <param name="isFixed"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public MvcHtmlString Breadcrumbs(string iconClass, List<Nav> items, bool isFixed = false, MvcHtmlString append = null)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("data 参数不能为空！");
            var html = new StringBuilder();
            html.AppendFormat("<div class=\"breadcrumbs{0}\" id=\"breadcrumbs\">", isFixed ? " breadcrumbs-fixed" : string.Empty);
            html.Append("<script>try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }</script>");
            html.Append("<ul class=\"breadcrumb\">");
            var flag = 0;
            var icon = string.Format("<i class=\"ace-icon fa {0}\"></i>", iconClass);
            foreach (var item in items)
            {
                if (flag == items.Count - 1)
                    html.AppendFormat("<li class=\"active\"> {0} {1}</li>", flag == 0 ? icon : string.Empty, item.Text);
                else
                    html.AppendFormat("<li>{0}<a href = \"{1}\" > {2} </a></li>", flag == 0 ? icon : string.Empty, item.Url, item.Text);
                flag++;
            }
            html.Append("</ul>");
            if (append != null) html.Append(append.ToString());
            html.Append("</div>");
            return MvcHtmlString.Create(html.ToString());
        }

        public MvcHtmlString Sidebar(bool isFixed = false, bool isTopMenu = false, List<Shortcut> shortcuts = null, List<Menu> menus = null)
        {
            if (menus == null || menus.Count == 0)
                throw new ArgumentException("参数 menus 不能为空！");

            var html = new StringBuilder();
            html.AppendFormat("<div id = \"sidebar\" class=\"sidebar{0}{1}\">",
                isTopMenu ? " h-sidebar navbar-collapse collapse" : " responsive",
                isFixed ? " fixed sidebar-fixed" : string.Empty);
            html.Append("<script>try { ace.settings.check('sidebar', 'fixed') } catch (e) { }</script>");

            if (shortcuts != null) CreateSidebarShortcuts(html, shortcuts);

            // sidebar menu
            html.Append("<ul class=\"nav nav-list\">");
            CreateSidebarMenu(html, menus, isTopMenu);
            html.Append("</ul>");

            // toggle button
            html.Append("<div class=\"sidebar-toggle sidebar-collapse\" id=\"sidebar-collapse\">");
            html.Append("<i class=\"ace-icon fa fa-angle-double-left\" data-icon1=\"ace-icon fa fa-angle-double-left\" data-icon2=\"ace-icon fa fa-angle-double-right\"></i>");
            html.Append("</div>");
            html.Append("<script>try { ace.settings.check('sidebar', 'collapsed') } catch (e) { }</script>");

            html.Append("</div>");

            return MvcHtmlString.Create(html.ToString());
        }

        private void CreateSidebarShortcuts(StringBuilder html, List<Shortcut> shortcuts)
        {
            html.Append("<div class=\"sidebar-shortcuts\" id=\"sidebar-shortcuts\">");
            // large
            html.Append("<div class=\"sidebar-shortcuts-large\" id=\"sidebar-shortcuts-large\">");
            foreach (var item in shortcuts)
            {
                html.AppendFormat("<button class=\"btn {0}\"><i class=\"ace-icon fa {1}\"></i></button>", item.ButtonClass, item.IconClass);
            }
            html.Append("</div>");
            // mini
            html.Append("<div class=\"sidebar-shortcuts-mini\" id=\"sidebar-shortcuts-mini\">");
            foreach (var item in shortcuts)
            {
                html.AppendFormat("<span class=\"btn {0}\"></span>", item.ButtonClass);
            }
            html.Append("</div>");
            // end
            html.Append("</div>");
        }
        private void CreateSidebarMenu(StringBuilder html, List<Menu> menus, bool hover)
        {
            // var tmpl =
            //@"<li class=''>
            //    <a href = '@Model.Url' >
            //        <i class='menu-icon fa @Model.PrimaryIcon'></i>
            //        <span class='menu-text'> @Model.Text</span>
            //        <b class='arrow fa fa-angle-down @Model.SecondIcon'></b>
            //    </a>
            //    <b class='arrow'></b>
            //</li>";
            foreach (var item in menus)
            {
                var attrs = new StringBuilder();
                if (item.HtmlAttributes != null)
                {
                    foreach (var p in item.HtmlAttributes.GetType().GetProperties())
                    {
                        attrs.AppendFormat(" {0}=\"{1}\"", p.Name.StartsWith("data_") ? p.Name.Replace("_", "_") : p.Name, p.GetValue(item.HtmlAttributes));
                    }
                }
                var attrstr = attrs.ToString();
                if (hover)
                {
                    if (attrstr.Contains("class"))
                    {
                        attrstr = Regex.Replace(attrstr, "class=\"([^\"]*)\"", "class=\"$1 hover\"");
                    }
                    else {
                        attrstr += " class=\"hover\"";
                    }
                }
                html.AppendFormat("<li{0}>", attrstr);
                html.AppendFormat("<a href = \"{0}\" {1}>", item.Url ?? "#", item.Menus.IsEmpty() ? string.Empty : "class=\"dropdown-toggle\"");
                if (!item.Icon.IsEmpty()) html.AppendFormat("<i class=\"menu-icon fa {0}\"></i>", item.Icon);
                html.AppendFormat("<span class=\"menu-text\">{0}</span>", item.Text);
                if (!item.Menus.IsEmpty())
                    html.Append("<b class=\"arrow fa fa-angle-down\"></b>");
                //else if (!item.SecondIcon.IsEmpty())
                //    html.AppendFormat("<b class=\"fa {0}\"></b>", item.SecondIcon);
                html.Append("</a>");
                html.Append("<b class=\"arrow\"></b>");
                if (!item.Menus.IsEmpty())
                {
                    html.Append("<ul class=\"submenu\">");
                    CreateSidebarMenu(html, item.Menus, hover);
                    html.Append("</ul>");
                }
                html.Append("</li>");
            }
        }

    }
}
