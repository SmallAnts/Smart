using System;
using Smart.Core.Extensions;
using System.Web.Mvc;
using Smart.Web.Mvc;
using System.Collections.Generic;
using Smart.Web.Mvc.UI.JqGrid;
using Smart.Samples.Services.Extensions;
using System.Threading.Tasks;

namespace Smart.Samples.Web.Controllers
{
    public class HomeController : BaseController
    {
        Services.IUserService _userService;
        public HomeController(
            Services.IUserService userService
            )
        {
            _userService = userService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FirstGrid(int page, int rows)
        {
            var list = new List<dynamic>();
            list.Add(new
            {
                CategoryName = "Produce",
                ProductName = "Uncle Bob's Organic Dried Pears",
                Country = "UK",
                Price = 340.00,
                Quantity = 134
            });
            list.Add(new
            {
                CategoryName = "Produce",
                ProductName = "Manjimup Dried Apples",
                Country = "UK",
                Price = 38.0,
                Quantity = 34
            });
            return Json(new GridData { Total = 100, Page = page, Rows = list });
        }

        [HttpPost]
        public ActionResult Index(string id)
        {
            return View();
        }
        public ActionResult LeftSide() { return PartialView("_LeftSide"); }
        public ActionResult RightSide()
        {
            return PartialView("_RightSide");
        }

        public ActionResult SignIn() { return View(); }
        [HttpPost]
        public async Task<ActionResult> SignIn(Models.SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userService.SiginInAsync(model.Email, model.Password);
            if (user != null)
            {
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "邮箱地址或密码输入错误！");
            }
            return View(model);
        }

        public ActionResult ForgotPassword() { return View(); }

    }
}