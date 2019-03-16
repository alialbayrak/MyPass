using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using MyPass.Bll;
using MyPass.Entities;

namespace MyPass.Web.Model
{
    public class CacheHelper
    {
        public static List<Group> GetGroupsByUserId(int userId)
        {
            List<Group> groups = WebCache.Get("Groups");

            if (groups != null)
            {
                return groups;
            }

            GroupManager groupManager = new GroupManager();
            groups = groupManager.FindAll(userId);
            WebCache.Set("Groups", groups);

            return groups;

        }

        public static void RemoveGroups()
        {
            WebCache.Remove("Groups");
        }
    }
}