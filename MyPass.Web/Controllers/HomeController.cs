using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass;
using MyPass.Bll;
using MyPass.Entities;
using MyPass.Entities.ViewModel;
using MyPass.Web.Filter;
using MyPass.Web.Model;

namespace MyPass.Web.Controllers
{
    public class HomeController : Controller
    {
        HomeManager _bll = new HomeManager();

        [AuthFilter]
        public ActionResult Dashboard()
        {
            HomeDashboardViewModel model = new HomeDashboardViewModel();


            int currentUserId = SessionHelper.GetCurrentUser().Id;
            model = _bll.GetDashboardStats(currentUserId);
            return View(model);
        }

        public ActionResult Index()
        {
            if (SessionHelper.GetCurrentUser() != null)
                return RedirectToAction("Dashboard");

            return View();
        }

    }
}