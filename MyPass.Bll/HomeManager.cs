using MyPass.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Bll
{
    public class HomeManager
    {
        public int GetTotalGroupCount(int userId)
        {
            GroupDal groupDal = new GroupDal();
            return groupDal.GetTotalGroupCountByUserId(userId);

            
        }

        public int GetTotalItemCount(int userId)
        {
            ItemDal itemDal = new ItemDal();
            return itemDal.GetTotalItemCountByUserId(userId);

        }
    }
}
