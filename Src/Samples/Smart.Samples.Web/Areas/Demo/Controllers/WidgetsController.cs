using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.Demo.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Demo/Widgets
        public ActionResult Index()
        {
            return View();
        }
    }
}