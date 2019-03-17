using MyPass.Dal.Abstract;
using MyPass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal
{
    public class GroupDal : IDalBase<Group>
    {
        MyPassContext db = new MyPassContext();


        public List<Group> GetAll(int userId)
        {
            List<Group> groups = new List<Group>();
            groups = db.Groups.Where(m => m.Status == true).ToList();
            foreach (var group in groups)
            {
                group.ItemList = group.ItemList.Where(m => m.Status == true).ToList();
            }
            return groups;
        }

        public Group GetById(int id, int userId)
        {
            return db.Groups.Find(id);
        }

        public int Add(Group param)
        {
            param.AddedDate = DateTime.Now;
            param.Status = true;

            db.Groups.Add(param);
            return db.SaveChanges();
        }

        public int Update(Group model)
        {
            model.UpdatedDate = DateTime.Now;
            var entry = db.Entry(model);
            entry.State = EntityState.Modified;
            entry.Property(m => m.OwnerUserId).IsModified = false;

            entry.Property(m => m.AddedDate).IsModified = false;
            entry.Property(m => m.Status).IsModified = false;

            return db.SaveChanges();
        }

        public int Delete(Group model)
        {
            model.Status = false;
            model.UpdatedDate = DateTime.Now;
            var entry = db.Entry(model);
            entry.State = EntityState.Modified;

            entry.Property(m => m.AddedDate).IsModified = false;

            return db.SaveChanges();
        }

        public List<Item> GetGroupItems(int groupId)
        {
            return db.Items.Where(m => m.GroupId == groupId && m.Status == true).ToList();
        }

        public int GetTotalGroupCountByUserId(int userId)
        {
            return db.Groups.Join(db.GroupUsers, group => group.Id, groupUser => groupUser.GroupId, (group, groupUser) => new { group, groupUser })
                .Where(m => m.groupUser.UserId == userId && m.group.Status == true).Select(m => m.group).ToList().Count();
        }

        #region GroupUser

        public int AddGroupUser(GroupUser model)
        {
            db.GroupUsers.Add(model);
            return db.SaveChanges();
        }

        public GroupUser GetGroupUserByUserIdGroupId(int userId, int groupId)
        {
            return db.GroupUsers.FirstOrDefault(m => m.GroupId == groupId && m.UserId == userId);
        }

        public List<GroupUser> GetAllGroupUsers(int groupId)
        {
            return db.GroupUsers.Where(m => m.GroupId == groupId).ToList();
        }

        public void DeleteGroupUser(GroupUser model)
        {
            db.GroupUsers.Remove(model);
            db.SaveChanges();
        }

        #endregion

    }
}
