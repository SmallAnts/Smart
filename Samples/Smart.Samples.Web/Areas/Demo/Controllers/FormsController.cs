using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.Demo.Controllers
{
    public class FormsController : Controller
    {
        public ActionResult Editor() { return View(); }
        public ActionResult Elements() { return View(); }
        public ActionResult Elements2() { return View(); }
        public ActionResult Upload() { return View(); }
        public ActionResult Validation() { return View(); }
    }
}