using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Entities.ViewModel
{
    public class CategoryDetailViewModel
    {
        public Category Category { get; set; }
        public List<User> SharedCategoryUserList { get; set; }

        public string CategoryOwnerName { get; set; }      
    }
}
