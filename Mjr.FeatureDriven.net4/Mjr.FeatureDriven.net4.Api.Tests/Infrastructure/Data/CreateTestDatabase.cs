using System.Data.Entity;
using Mjr.FeatureDriven.net4.Api.Data;
using Mjr.FeatureDriven.net4.Api.Database.Seeding;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Data
{
    /// <summary>
    /// This tests recreates the database and runs the seed that is configured in the database project.
    /// </summary>
    public class CreateTestDatabase : CreateDatabaseIfNotExists<ApiContext>
    {
        protected override void Seed(ApiContext context)
        {
            Seeder.Seed(context);
        }
    }
}
