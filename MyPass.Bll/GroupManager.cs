using MyPass.Entities;
using MyPass.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPass.Bll.Helper;

namespace MyPass.Bll
{
    public class GroupManager
    {
        private GroupDal _dal = new GroupDal();

        public int AddGroup(Group group)
        {
            return _dal.Add(group);
        }

        public List<Group> GetAllUserGroups(int userId)
        {
            return _dal.GetGroupsByUserId(userId);
        }

        public Group FindGroup(int groupId, int userId)
        {
            Group group = _dal.GetById(groupId, userId);
            FillGroupItems(group);

            return group;
            
        }

        public int RemoveGroup(int groupId, int userId)
        {
            int id = 0;
            Group group = _dal.GetById(groupId, userId);
            if (group != null)
            {
                id = _dal.Delete(group);
                if (id == 0)
                    throw new Exception("Grup silinemedi!");
                
            }
            else
                throw new Exception("Grup bulunamadı!");

            return id;

        }
        
        public int UpdateGroup(Group group)
        {
            return _dal.Update(group);
        }

        public Group FillGroupItems(Group group)
        {
            group.ItemList = _dal.GetGroupItems(group.Id);

            foreach (var itemElem in group.ItemList)
            {
                itemElem.Password = SecurityHelper.Decode(itemElem.Password);
            }

            return group;
        }
    }
}
