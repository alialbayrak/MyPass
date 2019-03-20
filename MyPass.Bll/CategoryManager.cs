using MyPass.Entities;
using MyPass.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPass.Bll.Helper;
using MyPass.Entities.ViewModel;
using MyPass.Dal.EntityFramework;

namespace MyPass.Bll
{
    public class CategoryManager
    {
        private MyPassContext db = new MyPassContext();
        Repository<Category> categoryRepository = new Repository<Category>();
        Repository<User> userRepository = new Repository<User>();
        Repository<Item> itemRepository = new Repository<Item>();
        Repository<CategoryUser> categoryUserRepository = new Repository<CategoryUser>();


        public Category Find(int categoryId, int currentUserId)
        {
            Category category = categoryRepository.GetById(categoryId);

            if (category != null)
            {
                category = category.CategoryUsers.Where(m => m.UserId == currentUserId && m.Status == true).Select(m => m.Category).FirstOrDefault();
            }

            return category;
        }

        public List<Category> FindAll(int userId)
        {
            List<int> categoryUserIds = categoryUserRepository.Get(m => m.UserId == userId && m.Status == true).Select(m => m.CategoryId).ToList();
            return categoryRepository.Get(m => m.Status == true && categoryUserIds.Contains(m.Id));
        }

        public int Add(Category category, int userId)
        {
            using (var dbTransaction = db.Database.BeginTransaction())
            {
                Repository<CategoryUser> categoryUserRepository = new Repository<CategoryUser>(db);
                Repository<Category> categoryRepository = new Repository<Category>(db);

                categoryRepository.Insert(category);

                //kategori kullanıcısı ekleme
                CategoryUser categoryUser = new CategoryUser
                {
                    CategoryId = category.Id,
                    UserId = userId,
                    IsOwner = true,
                };
                categoryUserRepository.Insert(categoryUser);


                dbTransaction.Commit();

                return category.Id;
            }

        }

        public int Delete(int categoryId, int currentUserId)
        {
            CategoryUser categoryUser = categoryUserRepository.Get(m => m.CategoryId == categoryId && m.UserId == currentUserId).FirstOrDefault();

            if (categoryUser != null)
            {
                if (categoryUser.IsOwner)
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        Repository<Item> itemRepository = new Repository<Item>(db);
                        Repository<CategoryUser> categoryUserRepository = new Repository<CategoryUser>(db);
                        Repository<Category> categoryRepository = new Repository<Category>(db);

                        Category category = categoryRepository.GetById(categoryId);

                        foreach (var item in category.ItemList)
                        {
                            itemRepository.Delete(item);
                        }

                        foreach (var user in category.CategoryUsers)
                        {
                            categoryUserRepository.Delete(user);
                        }

                        categoryRepository.Delete(category);

                        dbTransaction.Commit();

                        return category.Id;
                    }

                }
                else
                    throw new Exception("Grup sahibi olmadan silemezsiniz!");
            }
            else
                throw new Exception("Grup bulunamadı!");

        }

        public int Update(Category group)
        {
            return categoryRepository.Update(group);
        }

        public void ShareCategory(string email, int categoryId, int currentUserId) // email ekenecek
        {
            User user = userRepository.Get(m => m.Email == email).FirstOrDefault();

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (user.Id == currentUserId)
                throw new Exception("Kendinizi ekleyemezsiniz!");

            Category category = categoryRepository.GetById(categoryId);

            CategoryUser categoryUser = new CategoryUser();
            categoryUser.CategoryId = category.Id;
            categoryUser.UserId = user.Id;

            categoryUserRepository.Insert(categoryUser);
        }

        public void UnShareCategory(int userId, int categoryId, int currentUserId) //userId çıkarılacak olan
        {
            CategoryUser categoryUser = categoryUserRepository.Get(m => m.UserId == userId && m.CategoryId == categoryId).FirstOrDefault();
            CategoryUser owner = categoryUserRepository.Get(m => m.UserId == currentUserId && m.CategoryId == categoryId).FirstOrDefault();

            if (categoryUser == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (!owner.IsOwner)
                throw new Exception("Kategori sahibi değilsiniz!");

            categoryUserRepository.Delete(categoryUser);
        }

        public List<User> GetSharedCategoryUsers(int categoryId) // kategori paylaşılmış kişiler
        {
            List<int> userIds = categoryUserRepository.Get(m => m.CategoryId == categoryId && m.IsOwner == false && m.Status == true).Select(m => m.UserId).ToList();
            return userRepository.Get(m => userIds.Contains(m.Id));
        }

        public User GetOwnerUser(int categoryId)
        {
            return categoryUserRepository.Get(m => m.CategoryId == categoryId && m.IsOwner == true).FirstOrDefault().User;
        }

        public List<User> GetAllCategoryUsers(int categoryId)
        {
            List<User> users = new List<User>();
            List<CategoryUser> categoryUsers = categoryUserRepository.Get(m => m.CategoryId == categoryId);

            foreach (var categoryUser in categoryUsers)
            {
                users.Add(categoryUser.User);
            }

            return users;
        }

    }
}
