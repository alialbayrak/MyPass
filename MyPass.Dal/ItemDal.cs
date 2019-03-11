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
    public class ItemDal : IDalBase<Item>
    {
        new MyPassContext db = new MyPassContext();

        public int Add(Item model)
        {
            model.AddedDate = DateTime.Now;
            model.Status = true;
            db.Items.Add(model);
            return db.SaveChanges();
        }

        public int Delete(Item model)
        {
            throw new NotImplementedException();
        }

        public List<Item> GetAll(int userId)
        {
            throw new NotImplementedException();
        }

        public Item GetById(int id, int userId)
        {
            return db.Items.FirstOrDefault(m => m.Id == id && m.Status == true);

        }

        public int Update(Item model)
        {
            model.UpdatedDate = DateTime.Now;
            var entry = db.Entry(model);
            entry.State = EntityState.Modified;
            entry.Property(m => m.AddedDate).IsModified = false;
            entry.Property(m => m.GroupId).IsModified = false;
            entry.Property(m => m.ItemTypeId).IsModified = false;

            return db.SaveChanges();
        }

        public int GetTotalItemCountByUserId(int userId)
        {
            return db.Items.Join(db.Groups,item => item.GroupId, group => group.Id, (item, group) => new { item, group })
                .Where(m => m.group.UserId == userId && m.group.Status == true && m.item.Status == true).ToList().Count();
        }

    }
}
