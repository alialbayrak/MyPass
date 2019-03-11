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
    public class UserDal : IDalBase<User>
    {
        MyPassContext db = new MyPassContext();

        public int Add(User param)
        {
            param.AddedDate = DateTime.Now;
            param.Status = true;

            db.Users.Add(param);

            return db.SaveChanges();
        }

        public int Delete(User model)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id, int userId = 0)
        {
            return db.Users.FirstOrDefault(m => m.Id == id && m.Status == true);

        }

        public User GetUserByEmailAdress(string emailAdress)
        {
            return db.Users.FirstOrDefault(m => m.Email == emailAdress && m.Status == true);

        }

        public User GetUserbyEmailPassword(string emailAdress, string password)
        {
            return db.Users.FirstOrDefault(m => m.Email == emailAdress && m.Password == password && m.Status == true);
        }

        public int Update(User model)
        {
            model.UpdatedDate = DateTime.Now;
            var entry = db.Entry(model);
            entry.State = EntityState.Modified;
            entry.Property(m => m.AddedDate).IsModified = false;

            return db.SaveChanges();
        }
    }
}
