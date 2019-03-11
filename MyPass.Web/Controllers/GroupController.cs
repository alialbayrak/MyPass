using MyPass.Entities;
using MyPass.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass.Web.Filter;

namespace MyPass.Web.Controllers
{
    [AuthFilter]
    public class GroupController : Controller
    {
        GroupManager bll = new GroupManager();

        // GET: Group
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            List<Group> groups = bll.GetAllUserGroups((Session["User"] as User).Id);
            return View(groups);
        }

        #region Details
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Group group = bll.FindGroup((int)id, (Session["User"] as User).Id);
                return View(group);
            }
            else
                return new HttpStatusCodeResult(400, "Opps");

        }
        #endregion

        #region Create
        // GET: Group/Create
        public ActionResult Create()
        {

            return View(new Group());
        }

        // POST: Group/Create
        [HttpPost]
        public ActionResult Create(Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    group.UserId = (Session["User"] as User).Id;
                    bll.AddGroup(group);
                    (Session["GroupList"] as List<Group>).Add(group);

                    return RedirectToAction("Details", "Group", new { id = group.Id });
                }

                return View(group);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(group);
            }
        }
        #endregion

        #region Edit

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Group group = bll.FindGroup((int)id, (Session["User"] as User).Id);
                return View(group);
            }
            else
                return new HttpStatusCodeResult(400, "Opps");

        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bll.UpdateGroup(group);
                    return RedirectToAction("Details", new { id = group.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(group);
        }

        #endregion


        public ActionResult Delete(int? id)
        {
            try
            {
                if(id != null)
                {
                    bll.RemoveGroup((int)id, (Session["User"] as User).Id);
                    Session["GroupList"] = bll.GetAllUserGroups((Session["User"] as User).Id);
                }
                    
                else
                    return new HttpStatusCodeResult(400, "Opps");

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }


        #region Items

        public ActionResult Items(int? id)
        {
            //gruba bağlı itemları getir
            //view item list olarak göster
            return null;
        }

        #endregion

    }
}
