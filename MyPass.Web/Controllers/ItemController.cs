using MyPass.Bll;
using MyPass.Bll.Helper;
using MyPass.Entities;
using MyPass.Web.Filter;
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

        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            int userId = (Session["User"] as User).Id;
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

        // GET: Item/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Item/Create
        [HttpPost]
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
        public PartialViewResult SaveItem(Item item)
        {
            try
            {
                _bll.AddItem(item);
                GroupManager groupManager = new GroupManager();
                var group = new Group();
                group.Id = item.GroupId;
                group = groupManager.FillGroupItems(group);
                return PartialView("~/Views/Group/_GroupItemList.cshtml", group);
            }
            catch
            {
                return PartialView("Create", item);
            }
        }

        public string CopyPassword(int id)
        {
            return _bll.FindPassword(id);
        }
    }
}
