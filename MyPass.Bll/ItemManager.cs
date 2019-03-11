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
        private ItemDal _dal = new ItemDal();

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
    }
}
