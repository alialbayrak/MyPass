using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass;
using MyPass.Entities;
using MyPass.Web.Filter;

namespace MyPass.Web.Controllers
{
    public class HomeController : Controller
    {

        [AuthFilter]
        public ActionResult Dashboard()
        {
            //Toplam Grup Sayısı
            //Toplam Madde Sayısı
            return View();
        }

        public ActionResult Index()
        {
            if (Session["User"] != null)
                return RedirectToAction("Dashboard");

            return View();
        }
    }
}