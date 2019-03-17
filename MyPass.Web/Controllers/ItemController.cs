using MyPass.Bll;
using MyPass.Bll.Helper;
using MyPass.Entities;
using MyPass.Web.Filter;
using MyPass.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPass.Web.Controllers
{
    [AuthFilter]
    public class ItemController : Controller
    {
        private ItemManager _bll = new ItemManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            int userId = SessionHelper.GetCurrentUser().Id;
            Item item = _bll.FindItem(id, userId);
            item.Password = SecurityHelper.Decode(item.Password);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bll.UpdateItem(item);
                    return RedirectToAction("Details", "Group", new { id = item.GroupId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(item);
                }
            }

            return View(item);
        }

        public PartialViewResult Create(int id) 
        {
            Item item = new Item { GroupId = id, ItemTypeId = Item.ItemType.Password };
            try
            {
                return PartialView("Create", item);
            }
            catch
            {
                return PartialView("Create", item);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public PartialViewResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bll.AddItem(item);
                    GroupManager groupManager = new GroupManager();
                    Group group = groupManager.Find(item.GroupId, SessionHelper.GetCurrentUser().Id);
                    SessionHelper.RemoveGroups();
                    return PartialView("~/Views/Group/_GroupItemList.cshtml", group);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("",ex.Message);
                    return PartialView("Create", item);
                }
            }

            return PartialView("Create", item);

        }

        public string CopyPassword(int id)
        {
            return _bll.FindPassword(id);
        }

        public ActionResult Delete(int? id)
        {
            int groupId = 0;
            try
            {
                if (id != null)
                {
                    groupId = _bll.Remove((int)id, SessionHelper.GetCurrentUser().Id);
                    SessionHelper.RemoveGroups();
                }
                else
                    return new HttpStatusCodeResult(400, "Opps");

                return RedirectToAction("Detail","Group",new { id = groupId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
