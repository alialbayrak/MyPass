using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Entities.ViewModel
{
    public class GroupEditViewModel
    {
        public Category Group { get; set; }
        public List<User> SharedGroupUserList { get; set; }

        public string GroupOwnerName { get; set; }


    }

}
