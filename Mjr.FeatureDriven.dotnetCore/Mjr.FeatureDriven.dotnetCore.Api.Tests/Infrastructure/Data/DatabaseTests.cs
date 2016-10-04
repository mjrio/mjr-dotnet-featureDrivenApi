using System;

using System.Linq;
using Mjr.FeatureDriven.dotnetCore.Api.Data;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Data
{
    public class DatabaseTests
    {
        //Note : It will drop and recreate the entire database.So point to local test database!
        public void RecreateDatabase()
        {

            using (var dbContext = new UnitTestContext())
            {
                //dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
