using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Mjr.FeatureDriven.dotnetCore.Infrastructure.Database
{
    public abstract class BaseContext<TContext> : DbContext
         where TContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        protected BaseContext(DbContextOptions<TContext> options)
        : base(options)
        {
        }
        protected BaseContext()
        {


        }
        public void BeginTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    return;
                }

                _currentTransaction = Database.BeginTransaction();
            }
            catch (Exception)
            {
                // todo: log transaction exception
                throw;
            }
        }

        public void CloseTransaction()
        {
            CloseTransaction(exception: null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    // todo: log exception
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                if (_currentTransaction != null)
                {
                    _currentTransaction.Commit();
                }
            }
            catch (Exception)
            {
                // todo: log exception
                if (_currentTransaction != null && _currentTransaction.GetDbTransaction().Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
