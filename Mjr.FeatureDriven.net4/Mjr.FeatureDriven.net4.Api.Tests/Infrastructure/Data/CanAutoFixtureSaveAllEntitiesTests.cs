using System.Data.Entity;
using Mjr.FeatureDriven.net4.Api.Data.Entities;
using Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie;
using Shouldly;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Data
{
    public class CanAutoFixtureSaveAllEntitiesTests
    {
        public void Should_SaveEntity(TestContextFixture testContextFixture, Stock stock)
        {
            testContextFixture.SaveAll(stock);

        }

    }
}
