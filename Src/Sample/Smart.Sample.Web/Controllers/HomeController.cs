using Smart.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smart.Core.Extensions;

namespace Smart.Sample.Web.Controllers
{

    [FormAuthorize]
    public class HomeController : ControllerBase
    {
        // 框架面
        public ActionResult Index()
        {
            ViewBag.Menus = GetMenus();
            return View();
        }
        public ActionResult Index2()
        {
            ViewBag.Menus = GetMenus();
            return View();
        }
        // 首页
        public ActionResult FirstPage() { return View(); }

        #region 左侧菜单

        [HttpPost]
        //[OutputCache(Duration = 300)]
        public JsonResult Sidebar()
        {
            var menus = GetMenus();
            return Json(menus);
        }
        private List<Menu> GetMenus()
        {
            var service = this.GetService<Core.IServices.IUserService>();
            var funcs = service.GetUserFuncs(this.Operator.SysUserId);
            var menus = new List<Menu>();
            AddMenus(menus, funcs, ""); // 递归生成菜单列表
            return menus;
        }
        private void AddMenus(List<Menu> menus, List<Core.Entites.SysFunc> funcs, string parentId)
        {
            var fs = funcs.FindAll(f => f.ParentId == parentId);
            foreach (var fun in fs)
            {
                var menu = new Menu();
                menu.HtmlAttributes = "data-addtab=" + fun.SysFuncId + "";
                menu.Text = fun.Name.T();
                menu.Icon = fun.Icon;
                if (funcs.Exists(f => f.ParentId == fun.SysFuncId))
                {
                    menu.Menus = new List<Menu>();
                    AddMenus(menu.Menus, funcs, fun.SysFuncId);
                }
                else
                {
                    menu.Url = fun.Url;
                }
                menus.Add(menu);
            }
        }
        #endregion

        #region Demo

        [HttpPost]
        public JsonResult SidebarDemo()
        {
            var menus = new List<Menu>();

            menus.Add(new Menu { Url = "#/demo/charts/index", Icon = "fa-tachometer", Text = "图表" });
            menus.Add(new Menu { Url = "#/consume/pay", Icon = "fa-tachometer", Text = "test" });
            menus.Add(new Menu
            {
                Icon = "fa-desktop",
                Text = "界面布局",
                Menus = new List<Menu>
            {
                new Menu { Url = "#/demo/ui/topmenu", Icon = "fa-caret-right", Text = "顶部导航" },
                new Menu { Url = "#/demo/ui/typography", Icon = "fa-caret-right", Text = "排版" },
                new Menu { Url = "#/demo/ui/elements", Icon = "fa-caret-right", Text = "UI组件" },
                new Menu { Url = "#/demo/ui/buttons", Icon = "fa-caret-right", Text = "按钮" },
                new Menu { Url = "#/demo/ui/contentsilder", Icon = "fa-caret-right", Text = "滑动内容" },
                new Menu { Url = "#/demo/ui/treeview", Icon = "fa-caret-right", Text = "树" }
            }
            });
            menus.Add(new Menu
            {
                Icon = "fa-table",
                Text = "表格",
                Menus = new List<Menu>
            {
                new Menu { Url = "#/demo/tables/jqgrid", Icon = "fa-caret-right", Text = "JqGrid" },
            }
            });
            menus.Add(new Menu
            {
                Icon = "fa-pencil-square-o",
                Text = "表单",
                Menus = new List<Menu> {
                new Menu { Url = "#/demo/forms/elements", Icon = "fa-caret-right", Text = "组件" },
                new Menu { Url = "#/demo/forms/validation", Icon = "fa-caret-right", Text = "验证" },
                new Menu { Url = "#/demo/forms/editor", Icon = "fa-caret-right", Text = "富文本编辑" },
                new Menu { Url = "#/demo/forms/upload", Icon = "fa-caret-right", Text = "上传" },
            }
            });
            menus.Add(new Menu { Url = "#/demo/widgets/index", Icon = "fa-list-alt", Text = "插件" });
            menus.Add(new Menu { Url = "#/demo/calendar/index", Icon = "fa-calendar", Text = "日程安排" });
            menus.Add(new Menu { Url = "#/demo/gallery/index", Icon = "fa-picture-o", Text = "画廊" });
            menus.Add(new Menu
            {
                Icon = "fa-tag",
                Text = "其它页面",
                Menus = new List<Menu>
            {
                new Menu { Url = "#/demo/pages/faq", Icon = "fa-caret-right", Text = "常见问题" },
                new Menu { Url = "#/demo/pages/inbox", Icon = "fa-caret-right", Text = "收件箱" },
                new Menu { Url = "#/demo/pages/invoice", Icon = "fa-caret-right", Text = "购物车" },
                new Menu { Url = "#/demo/pages/pricingtables", Icon = "fa-caret-right", Text = "价目表" },
                new Menu { Url = "#/demo/pages/timeline", Icon = "fa-caret-right", Text = "时间轴" },
                new Menu { Url = "#/demo/pages/userProfile", Icon = "fa-caret-right", Text = "用户信息" },
            }
            });

            return Json(menus);
        }
        #endregion
    }
}