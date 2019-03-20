using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPass.Web.Controllers
{
    public class MyPassController : Controller
    {
        public ActionResult ErrorPage404()
        {
            return RedirectToAction("Page404", "Error");
        }


    }
}