using Smart.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Controllers
{
    public class AccountController : Smart.Web.Mvc.BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // Methods
        public ActionResult SignIn(string returnUrl = null)
        {
            // ViewData
            this.ViewData["ReturnUrl"] = returnUrl;

            // Return
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Models.SignInModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("index");
            }
            return View(model);
        }

        [HttpPost]
        [OutputCache(Duration = 300)]
        public JsonResult Sidebar()
        {
            var menus = new List<Menu>();
            menus.Add(new Menu { Url = "#/demo/charts/index", Icon = "fa-tachometer", Text = "图表" });
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
            var activeMenu = menus.Find(m => m.Url != null && this.Request.Url.AbsolutePath.ToString().StartsWith(m.Url));
            if (activeMenu != null)
            {
                activeMenu.HtmlAttributes = new { @calss = "active" };
            }

            return Json(menus);
        }

    }
}