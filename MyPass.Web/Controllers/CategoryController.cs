using MyPass.Entities;
using MyPass.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass.Web.Filter;
using MyPass.Entities.ViewModel;
using MyPass.Web.Model;
using System.Net;

namespace MyPass.Web.Controllers
{
    [AuthFilter]
    public class CategoryController : MyPassController
    {
        private CategoryManager _bll = new CategoryManager();

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            int currentUserId = SessionHelper.GetCurrentUser().Id;
            List<Category> groups = _bll.FindAll(currentUserId);
            return View(groups);
        }

        #region Detail
        public ActionResult Detail(int? id)
        {
            if (id != null)
            {
                int userId = SessionHelper.GetCurrentUser().Id;
                Category category = _bll.Find((int)id, userId);

                if (category == null)
                {
                    return ErrorPage404();
                }

                ItemManager itemManager = new ItemManager();
                category.ItemList = itemManager.EncodePasswords(category.ItemList);

                //ViewModel doldurma işlemleri.
                CategoryDetailViewModel model = new CategoryDetailViewModel()
                {
                    Category = category,
                    SharedCategoryUserList = _bll.GetSharedCategoryUsers(id.Value),
                    CategoryOwnerName = _bll.GetOwnerUser(id.Value).Email
                };
                return View(model);
            }
            else
                return ErrorPage404();

        }
        #endregion

        #region Create

        public ActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    int currentUserId = SessionHelper.GetCurrentUser().Id;

                    int categoryId = _bll.Add(category, currentUserId);

                    SessionHelper.RemoveCategories();

                    return RedirectToAction("Detail", "Category", new { id = categoryId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(category);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                int userId = SessionHelper.GetCurrentUser().Id;
                Category group = _bll.Find((int)id, userId);
                List<User> groupUserList = _bll.GetSharedCategoryUsers((int)id);

                GroupEditViewModel model = new GroupEditViewModel()
                {
                    Group = group,
                    SharedGroupUserList = groupUserList,
                    GroupOwnerName = _bll.GetOwnerUser(id.Value).Email
                };

                return View(model);
            }
            else
                return new HttpStatusCodeResult(400, "Opps");

        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(GroupEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category group = model.Group;
                    _bll.Update(group);
                    return RedirectToAction("Detail", new { id = group.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Share(string email, int groupId)
        {
            List<User> model = new List<User>();
            try
            {
                _bll.ShareCategory(email, groupId, SessionHelper.GetCurrentUser().Id);
                model = _bll.GetAllCategoryUsers(groupId);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_CategoryUserList", model);

        }

        [HttpPost]
        public ActionResult Unshare(int userId, int groupId)
        {
            List<User> model = new List<User>();
            try
            {
                _bll.UnShareCategory(userId, groupId, SessionHelper.GetCurrentUser().Id);
                model = _bll.GetAllCategoryUsers(groupId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_GroupUserList", model);

        }

        #endregion

        public ActionResult Delete(int? id)
        {
            try
            {
                if(id != null)
                {
                    _bll.Delete((int)id, SessionHelper.GetCurrentUser().Id);

                    SessionHelper.RemoveCategories();
                }                  
                else
                    return new HttpStatusCodeResult(400, "Opps");

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
