using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using MediatR;
using Mjr.FeatureDriven.net4.Api.Data;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Data;
using Mjr.FeatureDriven.net4.Api.Infrastructure.IoC;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Logging;
using Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Security;
using Ploeh.AutoFixture;
using Shouldly;
using FluentValidation;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public class TestContextFixture
    {
        private readonly StructureMapDependencyResolver _resolver;
        private IDependencyScope _scope;
        public TestUserSession UserSession;
        public IFixture Fixture;
        public TestContextFixture(StructureMapDependencyResolver resolver, IFixture fixture)
        {
            _resolver = resolver;
            Fixture = fixture;
        }

        public void SetUp()
        {
            _scope = _resolver.BeginScope();
            UserSession = (TestUserSession)_scope.GetService(typeof(IUserSession));
            LoggingConfig.Configure();
        }

        public object Get(Type type)
        {
            return _scope.GetService(type);
        }

        public void SaveAll(params object[] entities)
        {
            Do(dbContext =>
            {
                foreach (var entity in entities)
                {
                    var entry = dbContext.ChangeTracker.Entries().FirstOrDefault(entityEntry => entityEntry.Entity == entity);
                    if (entry == null)
                    {
                        dbContext.Set(entity.GetType()).Add(entity);
                    }
                }
            });
        }

        public void Reload<TEntity, TIdentity>(
            ref TEntity entity,
            TIdentity id)
            where TEntity : class
        {
            TEntity e = entity;

            Do(ctx => e = ctx.Set<TEntity>().Find(id));

            entity = e;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Do(dbContext => dbContext.Set<TEntity>().Remove(entity));
        }

        public void Do(Action action)
        {
            var dbContext = (BaseDbContext) _scope.GetService(typeof(BaseDbContext));
            try
            {
                dbContext.BeginTransaction();
                action();
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                HandleDbEntityValidationException(e);
                throw;
            }
        }

        public void Do(Action<DbContext> action)
        {
            var dbContext = (BaseDbContext)_scope.GetService(typeof(BaseDbContext));
            try
            {
                dbContext.BeginTransaction();
                action(dbContext);
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                HandleDbEntityValidationException(e);
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        public void DoClean(Action<DbContext> action)
        {
            using (var dbContext = new ApiContext())
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
                    HandleDbEntityValidationException(e);
                    throw;
                }
            }
        }

        public void ShouldValidate(object message)
        {
            var validator = Validator(message);
            validator.ShouldNotBeNull($"There is no validator for {message.GetType()} messages");
            var result = validator.Validate(message);
            var intentedErrorMessages = result
                .Errors
                .OrderBy(x => x.ErrorMessage)
                .Select(x => "    " + x.ErrorMessage)
                .ToArray();

            var actual = string.Join(Environment.NewLine, intentedErrorMessages);
            result.IsValid.ShouldBeTrue(
                $"Expected no validation errors, but found {result.Errors.Count}:{Environment.NewLine}{actual}");
        }

        public void ShouldNotValidate(object message, params string[] expectedErrors)
        {
            var validator = Validator(message);
            validator.ShouldNotBeNull($"There is no validator for {message.GetType()} messages");
            var result = validator.Validate(message);
            result.IsValid.ShouldBeFalse("Expected validation errors, but the message passed validation.");
            var actual = result
               .Errors
               .OrderBy(x => x.ErrorMessage)
               .Select(x => x.ErrorMessage)
               .ToArray();

            actual.ShouldBe(expectedErrors.OrderBy(x => x).ToArray());
        }
        public IValidator Validator(object message)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(message.GetType());
            return _scope.GetService(validatorType) as IValidator;
        }

        public void Send(IRequest message)
        {


            Send((IRequest<Unit>)message);
        }

        public TResult Send<TResult>(IRequest<TResult> message)
        {
            var validator = Validator(message);
            if (validator != null)
            {
                ShouldValidate(message);
            }
            var result = default(TResult);

            var context = (BaseDbContext)_scope.GetService(typeof(BaseDbContext));

            context.BeginTransaction();

            var mediator = (IMediator)_scope.GetService(typeof(IMediator));

            Exception exc = null;
            try
            {
                result = mediator.Send(message);
            }
            catch (Exception e)
            {
                exc = e;
            }

            context.CloseTransaction(exc);

            if (exc != null)
            {
                HandleDbEntityValidationException(exc);
            }

            return result;
        }

        public async Task SendAsync(IAsyncRequest message)
        {
            await SendAsync((IAsyncRequest<Unit>)message);
        }
        public async Task<TResult> SendAsync<TResult>(IAsyncRequest<TResult> message)
        {
            var validator = Validator(message);
            if (validator != null)
            {
                ShouldValidate(message);
            }
            var result = default(TResult);

            var context = (BaseDbContext)_scope.GetService(typeof(BaseDbContext));



            Exception exc = null;
            try
            {
                context.BeginTransaction();
                var mediator = (IMediator)_scope.GetService(typeof(IMediator));
                result = await mediator.SendAsync(message);
                context.CloseTransaction();

            }
            catch (Exception e)
            {
                context.CloseTransaction(e);
                HandleDbEntityValidationException(e);
                throw;
            }
            return result;
        }

        private void HandleDbEntityValidationException(Exception exception)
        {
            var dbException = exception as DbEntityValidationException;
            if (dbException != null)
            {
                var validationErrorMessage =
                    dbException.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors)
                        .Aggregate("", (current, validationError) => current + validationError.ErrorMessage);
                throw new Exception(validationErrorMessage, dbException);
            }
            throw exception;
        }
    }
}