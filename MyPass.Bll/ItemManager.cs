using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPass.Bll.Helper;
using MyPass.Dal;
using MyPass.Dal.EntityFramework;
using MyPass.Entities;

namespace MyPass.Bll
{
    public class ItemManager
    {
        private Repository<Item> itemRepository = new Repository<Item>();
        private Repository<Category> categoryRepository = new Repository<Category>();
        private Repository<CategoryUser> categoryUserRepository = new Repository<CategoryUser>();



        public Item Find(int itemId, int currentUserId)
        {
            Item item = itemRepository.GetById(itemId);
            int count = categoryUserRepository.Get(m => m.CategoryId == item.CategoryId && m.UserId == currentUserId).Count;

            if (count > 0)
                return null;

            return item;
        }

        public int Add(Item item)
        {
            string encodePass = SecurityHelper.Encode(item.Password);

            item.Password = encodePass;
            return itemRepository.Insert(item);
        }

        public int Update(Item item)
        {
            string encodePass = SecurityHelper.Encode(item.Password);

            item.Password = encodePass;
            return itemRepository.Update(item);
        }

        public string FindPassword(int id)
        {
            return DecodePassword(itemRepository.GetById(id).Password);
        }

        public int Delete(int itemId, int currentUserId)
        {
            Item item = itemRepository.GetById(itemId);

            if (item != null)
                return Delete(item, currentUserId);

            else
                throw new Exception("Madde bulunamadı!");
        }

        public int Delete(Item item, int currentUserId)
        {
            int categoryId = 0;
            CategoryUser categoryUser = categoryUserRepository.Get(m => m.CategoryId == item.CategoryId && m.UserId == currentUserId).FirstOrDefault();

            if (categoryUser != null)
            {
                if (categoryUser.IsOwner)
                {
                    itemRepository.Delete(item);
                    categoryId = item.CategoryId;
                }
                else
                    throw new Exception("Grup sahibi olmadan silemezsiniz!");
            }
            else
                throw new Exception("Bu maddeyi silemezsiniz!");

            return categoryId;
        }

        public string DecodePassword(string encodedPassword)
        {
            return SecurityHelper.Decode(encodedPassword);
        }

        public string EncodePassword(string password)
        {
            return SecurityHelper.Decode(password);
        }

        public List<Item> EncodePasswords(List<Item> items)
        {
            foreach (var item in items)
            {
                item.Password = EncodePassword(item.Password);
            }

            return items;
        }

    }
}
