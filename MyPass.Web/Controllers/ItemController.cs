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
    public class ItemController : MyPassController
    {
        private ItemManager _bll = new ItemManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
               
                int userId = SessionHelper.GetCurrentUser().Id;
                Item item = _bll.Find(id.Value, userId);

                if (item == null)
                {
                    return ErrorPage404();
                }

                item.Password = _bll.DecodePassword(item.Password);
                return View(item);

            }

            return ErrorPage404();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Update(item);
                    return RedirectToAction("Detail", "Category", new { id = item.CategoryId });
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
            Item item = new Item { CategoryId = id };
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
                    _bll.Add(item);
                    Category group = new CategoryManager().Find(item.CategoryId, SessionHelper.GetCurrentUser().Id);
                    //item.Password = _bll.DecodePassword(item.Password);
                    DecodeGroupItemsPassword(group); // grup içindeki şifreleri decode ediyor.
                    SessionHelper.RemoveCategories();
                    return PartialView("~/Views/Category/_CategoryItemList.cshtml", group);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return PartialView("Create", item);
                }
            }

            return PartialView("Create", item);

        }

        public Category DecodeGroupItemsPassword(Category group)
        {
            foreach (var item in group.ItemList)
            {
                item.Password = _bll.DecodePassword(item.Password);
            }

            return group;
        } //gönderilen grubun içindeki maddelerin şifrelerini decode ediyor.

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
                    groupId = _bll.Delete(id.Value, SessionHelper.GetCurrentUser().Id);
                    SessionHelper.RemoveCategories();
                }
                else
                    return new HttpStatusCodeResult(400, "Opps");

                return RedirectToAction("Detail", "Category", new { id = groupId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
