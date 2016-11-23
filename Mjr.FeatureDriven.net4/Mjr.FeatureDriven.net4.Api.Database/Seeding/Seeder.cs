using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mjr.FeatureDriven.net4.Api.Data;

namespace Mjr.FeatureDriven.net4.Api.Database.Seeding
{
    public static class Seeder
    {
        public static void Seed(ApiContext context)
        {

            try
            {

            }
            catch (Exception exception)
            {
                var dbException = exception as DbEntityValidationException;
                if (dbException != null)
                {
                    var validationErrorMessage = dbException.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate("", (current, validationError) => current + validationError.ErrorMessage);
                    Console.WriteLine(validationErrorMessage);
                }
                Console.WriteLine(exception.Message);
            }
        }
    }
}
