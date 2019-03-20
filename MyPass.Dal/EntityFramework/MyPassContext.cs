using MyPass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal.EntityFramework
{
    public class MyPassContext : DbContext
    {
        public MyPassContext()
        {
            Database.SetInitializer<MyPassContext>(new CreateDatabaseIfNotExists<MyPassContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<CategoryUser> CategoryUsers { get; set; }
    }


}
