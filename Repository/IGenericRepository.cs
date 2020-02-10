using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Repository
{
    
    public interface IGenericRepository<T> where T : class
    {
       
        IQueryable<T> GetAll();

        T Find(long Id);

        T Add(T entity);

    
        T Update(T entity);

        void Delete(long Id);

        void Delete(T entity);
    }
}
