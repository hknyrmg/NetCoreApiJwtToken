using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Context;
using TokenBasedAuth_NetCore.Repository;

namespace TokenBasedAuth_NetCore.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private  IDbContextTransaction _dbContextTransaction;
        private bool _disposed;

        public UnitOfWork(DbContext dbContext)
        {
            _context = dbContext;

        }
        public bool BeginNewTransaction()
        {
            try
            {
                _dbContextTransaction = _context.Database.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

      
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public bool RollBackTransaction()
        {
            try
            {
                _dbContextTransaction.Rollback();
                _dbContextTransaction = null;
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public int SaveChanges()
        {
            var transaction = _dbContextTransaction != null ? _dbContextTransaction : _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    //Context boş ise hata fırlatıyoruz
                    if (_context == null)
                    {
                        throw new ArgumentException("Context is null");
                    }

                   
                    //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                    int result = _context.SaveChanges();

                    //Sorun yok ise kuyruktaki tüm işlemleri commit ederek bitiriyoruz.
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    //Hata ile karşılaşılır ise işlemler geri alınıyor
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }
        #region DisposingSection

        /// <summary>
        /// Context ile işimiz bittiğinde dispose edilmesini sağlıyoruz
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DisposingSection
    }
}
