using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.AceHelp.Controllers
{
    public class BasicController : Controller
    {
        public ActionResult Layout() { return View(); }
        public ActionResult Content() { return View(); }
        public ActionResult Ajax() { return View(); }
        public ActionResult Footer() { return View(); }
        public ActionResult Navbar() { return View(); }
        public ActionResult Sidebar() { return View(); }
    }
}