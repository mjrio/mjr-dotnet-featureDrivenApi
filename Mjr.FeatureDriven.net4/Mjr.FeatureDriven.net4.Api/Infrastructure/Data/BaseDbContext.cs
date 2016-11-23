using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Data
{
    public abstract class BaseDbContext : DbContext
    {
        private DbContextTransaction _currentTransaction;
        protected BaseDbContext(string connectionString)
            : base(connectionString)
        {

            Database.Log = (
                (s) => ApiEventSource.Log.EntityFrameworkVerbose(s.SafeSubstring(200, ".."), s)
                );
        }

        public void BeginTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    return;
                }

                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception e)
            {
                ApiEventSource.Log.EntityFrameworkError(e.Message, e.GetContentOf());
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
            catch (Exception e)
            {
                ApiEventSource.Log.EntityFrameworkError(e.Message, e.GetContentOf());
                if (_currentTransaction != null && _currentTransaction.UnderlyingTransaction.Connection != null)
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