using MyPass.Dal;
using MyPass.Dal.EntityFramework;
using MyPass.Entities;
using MyPass.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Bll
{
    public class HomeManager
    {
        public HomeDashboardViewModel GetDashboardStats(int currentUserId)
        {
            Repository<CategoryUser> categoryUserRepository = new Repository<CategoryUser>();

            Repository<Category> categoryRepository = new Repository<Category>();
            Repository<Item> itemRepository = new Repository<Item>();

            var categoryUserList = categoryUserRepository.Get(m => m.Status == true && m.UserId == currentUserId).ToList();
            int categoryCount = categoryUserList.Count();

            int itemCount = 0;
            foreach (var item in categoryUserList)
            {
                itemCount += item.Category.ItemList.Count;
            }

            HomeDashboardViewModel model = new HomeDashboardViewModel();
            model.CategoryCount = categoryCount;
            model.ItemCount = itemCount;

            return model;


        }
    }
}
