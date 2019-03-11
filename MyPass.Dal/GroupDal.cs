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
        new MyPassContext db = new MyPassContext();

        public List<Group> GetAll(int userId)
        {
            return db.Groups.Where(m => m.UserId == userId && m.Status == true).ToList();
        }

        public Group GetById(int groupId, int userId)
        {
            return db.Groups.FirstOrDefault(m => m.Id == groupId && m.UserId == userId && m.Status == true);
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
            entry.Property(m => m.AddedDate).IsModified = false;
            entry.Property(m => m.UserId).IsModified = false;

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

        public List<Group> GetGroupsByUserId(int userId)
        {
            return db.Groups.Where(m => m.UserId == userId && m.Status == true).ToList();
        }

        public List<Item> GetGroupItems(int groupId)
        {
            return db.Items.Where(m => m.GroupId == groupId && m.Status == true).ToList();
        }

        public int GetTotalGroupCountByUserId(int userId)
        {
            return db.Groups.Where(m => m.UserId == userId && m.Status == true).ToList().Count();
        }


    }
}
