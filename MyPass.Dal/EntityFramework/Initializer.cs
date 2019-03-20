using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyPass.Entities;

namespace MyPass.Dal.EntityFramework
{
    class Initializer : CreateDatabaseIfNotExists<MyPassContext>
    {
        protected override void Seed(MyPassContext context)
        {
            base.Seed(context);

            User user = new User
            {
                Name = "admin",
                Surname = "admin",
                Email = "admin@email.com",
                Password = "MTIzNDU2", //123456
                IsActive = true,
                Status = true,
            };

            context.Users.Add(user);
            context.SaveChanges();
            
        }
    }
}
