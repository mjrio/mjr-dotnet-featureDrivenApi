using Ploeh.AutoFixture;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public abstract class IntegrationTestConvention : FixieConvention
    {
        protected IntegrationTestConvention()
        {
            Classes
                .AreInIntegrationTestNamespace();
        }

        protected override ICustomization AutoFixtureCustomization => new IntegrationTestsFixtureCustomization();
    }
    
}