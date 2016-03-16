using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.Demo.Controllers
{
    public class UIController : Controller
    {
        public ActionResult TopMenu() { return View(); }
        public ActionResult Typography() { return View(); }
        public ActionResult Elements() { return View(); }
        public ActionResult Buttons() { return View(); }
        public ActionResult ContentSilder() { return View(); }// 滑动的内容
        public ActionResult TreeView() { return View(); }

    }
}