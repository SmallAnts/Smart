using System;
using Smart.Core.Extensions;
using System.Web.Mvc;

namespace Smart.Samples.Web.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index(string id)
        {
            return View();
        }
        public ActionResult FluentValidation() { return View(); }

        [HttpPost]
        public ActionResult FluentValidation(Models.SignInModel userinfo)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(userinfo);
        }

        public ActionResult SignIn() { return View(); }
        [HttpPost]
        public ActionResult SignIn(Models.SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.SiginIn(model.Email, model.Password);
            if (user != null)
            {
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "邮箱地址或密码输入错误！");
            }
            return View(model);
        }

        public ActionResult LeftSide() { return PartialView("_LeftSide"); }
        public ActionResult RightSide()
        {
            return PartialView("_RightSide");
        }
    }
}