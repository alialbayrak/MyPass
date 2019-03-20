using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Dal.EntityFramework
{
    public class RepositoryBase
    {
        private static MyPassContext _db;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        public static MyPassContext CreateContext()
        {
            if (_db == null)
            {
                lock (_lockSync)
                {
                    if (_db == null)
                    {
                        _db = new MyPassContext();
                    }  
                }
            }

            return _db;
        }
    }
}
