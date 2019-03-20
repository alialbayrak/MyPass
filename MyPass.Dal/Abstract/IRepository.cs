using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal.Abstract
{
    public interface IRepository<T>
    {
        List<T> Get(Expression<Func<T, bool>> where);

        T GetById(int id);

        int Insert(T entity);

        int Delete(int id);

        int Delete(T entity);

        int Update(T entity);
    }
}
