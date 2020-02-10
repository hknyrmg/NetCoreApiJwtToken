using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Repository;

namespace TokenBasedAuth_NetCore.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<T> GetRepository<T>() where T : class;

        bool BeginNewTransaction();

        bool RollBackTransaction();

        int SaveChanges();

    }
}
