using MyPass.Bll;
using MyPass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MyPass.Web.Model
{
    public class SessionHelper
    {
        public static List<Group> GetGroupsByUserId(int userId)
        {
            List<Group> groups = HttpContext.Current.Session["Groups"] as List<Group>;

            if (groups != null)
            {
                return groups;
            }

            GroupManager groupManager = new GroupManager();
            groups = groupManager.FindAll(userId);
            HttpContext.Current.Session["Groups"] = groups;

            return groups;

        }

        public static void RemoveGroups()
        {
            HttpContext.Current.Session.Remove("Groups");
        }

        public static void AddCurrentUser(User user)
        {
            HttpContext.Current.Session["CurrentUser"] = user;
        }

        public static User GetCurrentUser()
        {
            return HttpContext.Current.Session["CurrentUser"] as User;
        }

    }
}