using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal.Abstract
{
    public interface IDalBase<T> where T : class
    {
        List<T> GetAll(int userId);

        T GetById(int id, int userId);

        int Add(T model);

        int Update(T model);

        int Delete(T model);
    }

}
