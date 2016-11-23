using System;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mjr.FeatureDriven.net4.Api.Data;
using Mjr.FeatureDriven.net4.Api.Data.Entities;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Data
{
    [TestClass]
    public class RecreateDatabaseByRunningThisTest
    {
        //Note : It will drop and recreate the entire database.So point to local test database!
        [TestMethod]
        public void RecreateDatabase()
        {
            using (var dbContext = new ApiContext())
            {
                try
                {
                    System.Data.Entity.Database.SetInitializer(new CreateTestDatabase());
                    dbContext.Stocks.ToList();
                }
                catch (Exception exception)
                {
                    var validationErrorMessage = exception.Message;

                    var dbException = exception as DbEntityValidationException;
                    if (dbException != null)
                        validationErrorMessage = dbException.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate("", (current, validationError) => current + validationError.ErrorMessage);
                    throw new Exception(validationErrorMessage);
                }

            }
        }

    }
}
