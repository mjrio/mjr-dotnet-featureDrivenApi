using System.Data.Entity;
using Mjr.FeatureDriven.net4.Api.Data;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Data
{
    public class CreateTestDatabase : CreateDatabaseIfNotExists<ApiContext>
    {
        protected override void Seed(ApiContext context)
        {
           // Seeder.Seed(context);
        }
    }
}
