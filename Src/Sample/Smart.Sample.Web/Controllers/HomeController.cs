using Smart.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smart.Core.Extensions;

namespace YQ.Cashier.Web.Controllers
{

    [FormAuthorize]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.Menus = GetMenus();
            return View("SpaIndex");
        }
        public ActionResult SpaIndex()
        {
            return Index();
        }
        public ActionResult FirstPage() { return View(); }

        [HttpPost]
        //[OutputCache(Duration = 300)]
        public JsonResult Sidebar()
        {
            var menus = GetMenus();
            return Json(menus);
        }
        private List<Menu> GetMenus()
        {
            var service = this.GetService<Services.IPrivilegeService>();
            var funcs = service.GetUserFunctions(this.Operator.SysUserId);
            var menus = new List<Menu>();
            AddMenus(menus, funcs, ""); // 递归生成菜单列表
            return menus;
        }
        private void AddMenus(List<Menu> menus, List<Domain.Entites.SysFunction> funcs, string parentId)
        {
            var fs = funcs.FindAll(f => f.ParentId == parentId);
            foreach (var fun in fs)
            {
                var menu = new Menu();
                menu.Text = fun.Name.T();
                menu.Icon = fun.Icon;
                if (funcs.Exists(f => f.ParentId == fun.SysFunctionId))
                {
                    menu.Menus = new List<Menu>();
                    AddMenus(menu.Menus, funcs, fun.SysFunctionId);
                }
                else
                {
                    menu.Url = "#" + fun.Url;
                }
                menus.Add(menu);
            }
        }


        #region Demo

        [HttpPost]
        [OutputCache(Duration = 300)]
        public JsonResult Sidebar1()
        {
            var menus = new List<Menu>();

            menus.Add(new Menu { Url = "#/home/firstpage", Icon = "fa-home", Text = "Home".T() });
            menus.Add(new Menu
            {
                Icon = "fa-shopping-cart",
                Text = "Cashiering".T(),
                Menus = new List<Menu>
                {
                    new Menu { Url = "#/cashier/payment", Icon = "fa-caret-right", Text = "Payment".T() },
                    new Menu {
                        Icon = "fa-caret-right",
                        Text = "Bills".T(),
                        Menus = new List<Menu> {
                            new Menu { Url = "#/cashier/bill_consume", Icon = "fa-caret-right", Text = "Consume bill".T() },
                            new Menu { Url = "#/cashier/bill_opencard", Icon = "fa-caret-right", Text = "Opencard bill".T() },
                            new Menu { Url = "#/cashier/bill_recharge", Icon = "fa-caret-right", Text = "Recharge bill".T() },
                            new Menu { Url = "#/cashier/bill_repayment", Icon = "fa-caret-right", Text = "Repayment bill".T() },
                            new Menu { Url = "#/cashier/bill_undo", Icon = "fa-caret-right", Text = "Undo bill".T() },
                        }
                    }
                }
            });

            menus.Add(new Menu
            {
                Icon = "fa-users",
                Text = "Members".T(),
                Menus = new List<Menu>
                {
                    new Menu { Url = "#/member/cardlist", Icon = "fa-caret-right", Text = "Card list".T() },
                    new Menu { Url = "#/member/customerlist", Icon = "fa-caret-right", Text = "Customer list".T()},
                }
            });

            menus.Add(new Menu
            {
                Icon = "fa-male",
                Text = "Employees".T(),
                Menus = new List<Menu>
                {
                    new Menu { Url = "#/Employee/employeeinfo", Icon = "fa-caret-right", Text = "Employee info".T() },
                    new Menu
                    {
                        Icon = "fa-caret-right",
                        Text = "Performance Set".T(),
                        Menus = new List<Menu>
                        {
                            new Menu { Url="#/Employee/PerformanceType",Icon ="fa-caret-right",Text="Performance type".T() },  //绩效类别
                            new Menu { Url="#/Employee/PerformanceTier",Icon="fa-caret-right",Text="Performance Tier".T() },   //岗位绩效阶梯
                            new Menu { Url="#/Employee/SalaryType",Icon = "fa-caret-right",Text="Salary type".T() }, //工资类别
                            new Menu { Url="#/Employee/SalaryEmail",Icon="fa-caret-right",Text="Salary Email".T() }   //工资邮件群发
                        }
                    },
                    //new Menu {
                    //    Icon = "fa-caret-right",
                    //    Text = "KPI Set".T(),
                    //    Menus = new List<Menu> {
                    //        new Menu { Url = "#/Employee/KPIType", Icon = "fa-caret-right", Text = "KPI type".T() },
                    //        new Menu { Url = "#/Employee/KPIStep", Icon = "fa-caret-right", Text = "KPI step".T()},
                    //        new Menu { Url = "#/Employee/PostKPIStep", Icon = "fa-caret-right", Text = "Post KPI step".T()},
                    //    }
                    //}
                }
            });

            menus.Add(new Menu
            {
                Icon = "fa-university",
                Text = "Inventory".T(),
                Menus = new List<Menu>
                {
                    new Menu { Url = "#/inventory/instock", Icon = "fa-caret-right", Text = "Instock".T() },
                    new Menu { Url = "#/inventory/outstock", Icon = "fa-caret-right", Text = "Outstock".T() },
                    new Menu { Url = "#/inventory/inventorytaking", Icon = "fa-caret-right", Text = "Inventory taking".T() },
                    //new Menu { Url = "#/inventory/eearlywarning", Icon = "fa-caret-right", Text = "Eearly warning".T() },
                }
            });

            menus.Add(new Menu
            {
                Icon = "fa-bar-chart",
                Text = "Income expenses".T(),
                Menus = new List<Menu> {
                    new Menu { Url = "#/incomeexpenses/create",Icon="fa-caret-right",Text="Billing".T() },
                    new Menu { Url = "#/incomeexpenses/index",Icon="fa-caret-right",Text="Bills".T() },
                }
            });

            menus.Add(new Menu { Url = "#/reports/reports", Icon = "fa-line-chart", Text = "Reports".T() });

            menus.Add(new Menu
            {
                Icon = "fa-calendar",
                Text = "Workforce".T(),
                Menus = new List<Menu>
                {
                    new Menu { Url = "#/Workforce/time", Icon = "fa-caret-right", Text = "门店营业时间" },
                    new Menu { Url = "#/Workforce/pay", Icon = "fa-caret-right", Text = "员工排班" },
                    new Menu {
                        Icon = "fa-caret-right",
                        Text = "员工考勤",
                        Menus = new List<Menu> {
                            new Menu { Url = "#/Workforce/view", Icon = "fa-caret-right", Text = "查看考勤" },
                            new Menu { Url = "#/Workforce/door", Icon = "fa-caret-right", Text = "门禁考勤" },
                            new Menu { Url = "#/Workforce/sp", Icon = "fa-caret-right", Text = "特殊考勤" },
                        }
                    }
                }
            });

            menus.Add(new Menu
            {
                Icon = "fa-database",
                Text = "Basic Data".T(),
                Menus = new List<Menu>
                {
                    new Menu { Icon = "fa-caret-right", Text = "Salon".T(), Menus=new List<Menu> () {
                        new Menu() { Url = "#/basicdata/storeinfo", Text="Salon info".T()},
                        new Menu() { Url = "#/basicdata/deptinfo", Text="Department".T()},
                        new Menu() { Url = "#/basicdata/payinfo", Text="Job".T()},
                    } },
                    new Menu {  Icon = "fa-caret-right", Text = "Services".T() , Menus=new List<Menu>() {
                        new Menu() { Url = "#/basicdata/servicetype", Text="Service type".T()},
                        new Menu() { Url = "#/basicdata/serviceinfo", Text="Service info".T()}
                    } },
                    new Menu { Icon = "fa-caret-right", Text = "Products", Menus=new List<Menu>() {
                        new Menu() { Url = "#/basicdata/productbrand", Text="Product brand".T()},
                        new Menu() { Url = "#/basicdata/productsupplier", Text="Product supplier".T()},
                        new Menu() { Url = "#/basicdata/productwarehouse", Text="Product warehouse".T()},
                        new Menu() { Url = "#/basicdata/producttype", Text="Product type".T()},
                        new Menu() { Url = "#/basicdata/productinfo",Text="Product info".T()}
                    } },
                    new Menu { Url = "#/basicdata/cardtype", Icon = "fa-caret-right", Text = "Card type" },
                    new Menu {
                        Icon = "fa-caret-right",
                        Text = "Other",
                        Menus = new List<Menu> {
                            new Menu { Url = "#/basicdata/storechannel", Icon = "fa-caret-right", Text = "Store channel".T() },//入店渠道维护
                            new Menu { Url = "#/basicdata/paymentmode", Icon = "fa-caret-right", Text = "Mode of payment".T() },//支付方式维护
                            new Menu { Url = "#/basicdata/scheduletype", Icon = "fa-caret-right", Text = "Scheduling type".T() },//排班类型维护
                            //new Menu { Url = "#/basicdata/messagetmpl", Icon = "fa-caret-right", Text = "SMS template".T() },//短信模板维护
                        }
                    }
                }
            });
            menus.Add(new Menu
            {
                Icon = "fa-cogs",
                Text = "System settings".T(),
                Menus = new List<Menu> {
                    new Menu {  Url = "#/sys/roles", Icon = "fa-caret-right", Text = "Roles".T() },
                    new Menu {  Url = "#/sys/sysusers", Icon = "fa-caret-right", Text = "Users".T() },
                    new Menu {  Url = "#/sys/settings", Icon = "fa-caret-right", Text = "Parameter settings".T() }
                }
            });

            return Json(menus);
        }

        [HttpPost]
        [OutputCache(Duration = 300)]
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
            var activeMenu = menus.Find(m => m.Url != null && this.Request.Url.AbsolutePath.ToString().StartsWith(m.Url));
            if (activeMenu != null)
            {
                activeMenu.HtmlAttributes = new { @calss = "active" };
            }

            return Json(menus);
        }
        #endregion
    }
}