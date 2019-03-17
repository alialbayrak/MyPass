using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPass.Bll.Helper;
using MyPass.Dal;
using MyPass.Entities;

namespace MyPass.Bll
{
    public class ItemManager
    {
        ItemDal _dal = new ItemDal();

        public Item FindItem(int itemId, int userId)
        {
            return _dal.GetById(itemId, userId);
        }

        public int AddItem(Item item)
        {
            string encodePass = SecurityHelper.Encode(item.Password);
            item.Password = encodePass;
            return _dal.Add(item);
        }

        public int UpdateItem(Item item)
        {
            string encodePass = SecurityHelper.Encode(item.Password);
            item.Password = encodePass;
            return _dal.Update(item);
        }

        public string FindPassword(int id)
        {
            return SecurityHelper.Decode(_dal.GetById(id, 0).Password);
        }

        public int Remove(int itemId, int currentUserId)
        {
            int groupId = 0;
            Item item = _dal.GetById(itemId, currentUserId);
            
            if (item != null)
            {
                GroupManager groupManager = new GroupManager();
                Group group = groupManager.Find(item.GroupId, currentUserId);
                if (group != null)
                {
                    if (group.OwnerUserId == currentUserId)
                    {
                        _dal.Delete(item);
                        groupId = group.Id;
                    }
                    else
                        throw new Exception("Grup sahibi olmadan silemezsiniz!");
                }
                else
                    throw new Exception("Bu maddeyi silemezsiniz!");
            }
            else
                throw new Exception("Madde bulunamadı!");

            return groupId;

        }
    }
}
