using MyPass.Entities;
using MyPass.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPass.Bll.Helper;
using MyPass.Entities.ViewModel;

namespace MyPass.Bll
{
    public class GroupManager
    {
        GroupDal _dal = new GroupDal();

        public List<Group> FindAll(int userId)
        {
            return _dal.GetAll(userId);
        }

        public int AddGroup(Group group)
        {
            int groupId = _dal.Add(group);

            return groupId;
        }

        public Group Find(int groupId, int userId)
        {
            Group group = _dal.GetById(groupId, userId);
            FillItems(group);

            return group;

        }

        public int Remove(int groupId, int userId)
        {
            int id = 0;
            Group group = _dal.GetById(groupId, userId);
            if (group != null)
            {
                GroupUserDal groupUserDal = new GroupUserDal();
                List<GroupUser> groupUsers = groupUserDal.GetByGroupId(groupId);
                
                id = _dal.Delete(group);

                if (groupUsers != null && groupUsers.Count > 0)
                {
                    foreach (var groupUser in groupUsers)
                    {
                        groupUserDal.Delete(groupUser);
                    }
                }

                if (id == 0)
                    throw new Exception("Grup silinemedi!");

            }
            else
                throw new Exception("Grup bulunamadı!");

            return id;

        }

        public int Update(Group group)
        {
            return _dal.Update(group);
        }

        public Group FillItems(Group group)
        {
            group.ItemList = _dal.GetGroupItems(group.Id);

            foreach (var itemElem in group.ItemList)
            {
                itemElem.Password = SecurityHelper.Decode(itemElem.Password);
            }

            return group;
        }

        public void ShareGroup(string email, int groupId, int currentUserId)
        {
            UserDal userDal = new UserDal();
            User user = userDal.GetUserByEmailAdress(email);

            if(user.Id == currentUserId)
                throw new Exception("Kendinizi ekleyemezsiniz!");

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            GroupUser groupUser = new GroupUser{
                GroupId = groupId,
                UserId = user.Id
            };
            _dal.AddGroupUser(groupUser);
        }

        public void UnShareGroup(int userId, int groupId, int currentUserId)
        {
            UserDal userDal = new UserDal();
            User user = userDal.GetById(userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            GroupUser groupUser = _dal.GetGroupUserByUserIdGroupId(userId, groupId);
            _dal.DeleteGroupUser(groupUser);
        }

        public List<User> FindGroupUsers(int groupId)
        {
            List<GroupUser> groupUserList = _dal.GetAllGroupUsers(groupId);
            List<User> UserList = new List<User>();
            if (groupUserList != null)
            {
                UserDal userDal = new UserDal();
                foreach (var groupUser in groupUserList)
                {
                    User user = new User();
                    user.Id = groupUser.UserId;
                    user.Email = userDal.GetById(groupUser.UserId).Email;

                    UserList.Add(user);
                }
            }

            return UserList;

        }

        public string FindOwnerEmail(int groupId)
        {
            Group group = _dal.GetById(groupId);

            UserDal userDal = new UserDal();
            User user = userDal.GetById(group.OwnerUserId);

            return user.Email;

        }
    }
}
