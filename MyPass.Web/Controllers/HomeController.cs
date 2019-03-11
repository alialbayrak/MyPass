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
            var ipAddress = string.Empty;
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (Request.ServerVariables["HTTP_CLIENT_IP"] != null && Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
                ipAddress = Request.ServerVariables["HTTP_CLIENT_IP"];
            else if (Request.UserHostAddress.Length != 0)
                ipAddress = Request.UserHostName;
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