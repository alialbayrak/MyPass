using MyPass.Dal.Abstract;
using MyPass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal
{
    public class GroupUserDal : IDalBase<GroupUser>
    {
        MyPassContext db = new MyPassContext();

        public int Add(GroupUser model)
        {
            db.GroupUsers.Add(model);
            return db.SaveChanges();
        }

        public int Delete(GroupUser model)
        {
            db.GroupUsers.Remove(model);
            return db.SaveChanges();
        }

        public List<GroupUser> GetAll(int userId)
        {
            throw new NotImplementedException();
        }

        public GroupUser GetById(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public List<GroupUser> GetByGroupId(int groupId)
        {
            return db.GroupUsers.Where(m => m.GroupId == groupId).ToList();
        }

        public int Update(GroupUser model)
        {
            throw new NotImplementedException();
        }
    }
}
