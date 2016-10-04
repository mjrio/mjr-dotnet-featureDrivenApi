using System;
using System.Linq;
using System.Threading.Tasks;
using Mjr.FeatureDriven.dotnetCore.Api.Data;
using MediatR;
using Ploeh.AutoFixture;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Data;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public class TestContextFixture
    {
        private readonly IContainer _resolver;
        private IContainer _scope;
        public IFixture Fixture;
        public TestContextFixture(IContainer resolver, IFixture fixture)
        {
            _resolver = resolver;
            Fixture = fixture;
        }

        public void SetUp()
        {
            _scope = _resolver.GetNestedContainer();
        }

        public object Get(Type type)
        {
            return _scope.GetInstance(type);
        }
        public T Get<T>()
        {
            return _scope.GetInstance<T>();
        }

        public void Save<TEntity>(TEntity entity)
            where TEntity : class
        {
            Do(dbContext =>
            {
                
                    var entry = dbContext.ChangeTracker.Entries().FirstOrDefault(entityEntry => entityEntry.Entity == entity);
                    if (entry == null)
                    {
                        dbContext.Set<TEntity>().Add(entity);
                    }
                
            });
        }

            public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Do(dbContext => dbContext.Set<TEntity>().Remove(entity));
        }

        public void Do(Action action)
        {
            var dbContext = (Context)Get(typeof(Context));
            try
            {
                dbContext.BeginTransaction();
                action();
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        public void Do(Action<DbContext> action)
        {
            var dbContext = Get<Context>();
            try
            {
                dbContext.BeginTransaction();
                action(dbContext);
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        public void DoClean(Action<DbContext> action)
        {
            using (var dbContext = new UnitTestContext())
            {
                try
                {
                    dbContext.BeginTransaction();
                    action(dbContext);
                    dbContext.CloseTransaction();
                }
                catch (Exception e)
                {
                    dbContext.CloseTransaction(e);
                    throw;
                }
            }
        }

      
        public void Send(IRequest message)
        {
            Send((IRequest<Unit>)message);
        }

        public TResult Send<TResult>(IRequest<TResult> message)
        {
            var context = Get<Context>();
            context.BeginTransaction();
            var mediator = Get<IMediator>();
            Exception exc = null;
            try
            {
                return mediator.Send(message);
            }
            catch (Exception e)
            {
                exc = e;
            }
            context.CloseTransaction(exc);
            if (exc != null)
            {
                throw exc;
            }
            return default(TResult); ;
        }

        public async Task SendAsync(IAsyncRequest message)
        {
            await SendAsync((IAsyncRequest<Unit>)message);
        }
        public async Task<TResult> SendAsync<TResult>(IAsyncRequest<TResult> message)
        {
            var result = default(TResult);
            var context = Get<Context>(); 
            try
            {
                context.BeginTransaction();
                var mediator = Get<IMediator>();
                result = await mediator.SendAsync(message);
                context.CloseTransaction();
            }
            catch (Exception e)
            {
                context.CloseTransaction(e);
                throw;
            }
            return result;
        }
    }
}