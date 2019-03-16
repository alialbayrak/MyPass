using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPass.Web.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View("Page404");
        }

        public ActionResult Page404()
        {
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }
    }
}