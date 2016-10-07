using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.Demo.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult FAQ() { return View(); }
        public ActionResult Inbox() { return View(); }
        public ActionResult Invoice() { return View(); }
        public ActionResult PricingTables() { return View(); }
        public ActionResult Timeline() { return View(); }
        public ActionResult UserProfile() { return View(); }
    }
}