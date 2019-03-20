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
        public static List<Category> GetCategoriesByUserId(int userId)
        {
            List<Category> groups = HttpContext.Current.Session["Categories"] as List<Category>;

            if (groups != null)
            {
                return groups;
            }

            CategoryManager groupManager = new CategoryManager();
            groups = groupManager.FindAll(userId);
            HttpContext.Current.Session["Categories"] = groups;

            return groups;

        }

        public static void RemoveCategories()
        {
            HttpContext.Current.Session.Remove("Categories");
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