using MyPass.Dal.Abstract;
using MyPass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal.EntityFramework
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private MyPassContext context;
        private DbSet<T> dbSet;

        public Repository()
        {
            this.context = new MyPassContext();
            this.dbSet = context.Set<T>();
        }

        public Repository(MyPassContext db)
        {
            this.context = db;
            this.dbSet = context.Set<T>();
        }


        public List<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public int Insert(T entity)
        {
            if (entity is EntityBase)
            {
                EntityBase o = entity as EntityBase;
                DateTime now = DateTime.Now;

                o.Status = true;
                o.CreatedOn = now;
                o.ModifiedUserEmail = "system";
            }
            dbSet.Add(entity);
            context.SaveChanges();
            return (entity as EntityBase).Id;
        }

        public int Delete(int id)
        {
            T entity = dbSet.Find(id);
            return Delete(entity);
        }

        public int Delete(T entity)
        {
            if (entity is EntityBase)
            {
                EntityBase o = entity as EntityBase;
                DateTime now = DateTime.Now;

                o.Status = false;
                o.ModifiedOn = now;
                o.ModifiedUserEmail = "system";
            }
            context.SaveChanges();
            return (entity as EntityBase).Id;
        }

        public int Update(T entity)
        {

            if (entity is EntityBase)
            {
                EntityBase o = entity as EntityBase;

                context.Entry(o).State = EntityState.Modified;
                context.Entry(o).Property(x => x.CreatedOn).IsModified = false;
                context.Entry(o).Property(x => x.Status).IsModified = false;
                context.Entry(o).Property(x => x.ModifiedUserEmail).IsModified = false;
                               
            }


            context.SaveChanges();
            return (entity as EntityBase).Id;
        }

    }
}
