using MyPass.Entities;
using MyPass.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass.Web.Filter;
using MyPass.Entities.ViewModel;

namespace MyPass.Web.Controllers
{
    [AuthFilter]
    public class GroupController : Controller
    {
        private GroupManager _bll = new GroupManager();

        // GET: Group
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            List<Group> groups = _bll.FindAll((Session["User"] as User).Id);
            return View(groups);
        }

        #region Detail
        public ActionResult Detail(int? id)
        {
            if (id != null)
            {
                int userId = (Session["User"] as User).Id;
                Group group = _bll.Find((int)id, userId);
                List<User> groupUserList = _bll.FindGroupUsers((int)id);
                GroupDetailViewModel model = new GroupDetailViewModel()
                {
                    Group = group,
                    SharedGroupUserList = groupUserList,
                    GroupOwnerName = _bll.FindOwnerEmail((int)id)
                };
                return View(model);
            }
            else
                return new HttpStatusCodeResult(400, "Opps");

        }
        #endregion

        #region Create

        public ActionResult Create()
        {

            return View(new Group());
        }


        [HttpPost]
        public ActionResult Create(Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    group.OwnerUserId = (Session["User"] as User).Id;
                    _bll.AddGroup(group);
                    if (Session["GroupList"] == null)
                        Session["GroupList"] = new List<Group>();

                    (Session["GroupList"] as List<Group>).Add(group);
                        

                    return RedirectToAction("Detail", "Group", new { id = group.Id });
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
                int userId = (Session["User"] as User).Id;
                Group group = _bll.Find((int)id, userId);
                List<User> groupUserList = _bll.FindGroupUsers((int)id);

                GroupEditViewModel model = new GroupEditViewModel()
                {
                    Group = group,
                    SharedGroupUserList = groupUserList,
                    GroupOwnerName = _bll.FindOwnerEmail((int)id)
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
                    Group group = model.Group;
                    _bll.Update(group);
                    return RedirectToAction("Details", new { id = group.Id });
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
                _bll.ShareGroup(email, groupId, (Session["User"] as User).Id);
                model = _bll.FindGroupUsers(groupId);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_GroupUserList", model);

        }

        [HttpPost]
        public ActionResult Unshare(int userId, int groupId)
        {
            List<User> model = new List<User>();
            try
            {
                _bll.UnShareGroup(userId, groupId, (Session["User"] as User).Id);
                model = _bll.FindGroupUsers(groupId);
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
                    _bll.Remove((int)id, (Session["User"] as User).Id);
                    Session["GroupList"] = _bll.FindAll((Session["User"] as User).Id);
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
