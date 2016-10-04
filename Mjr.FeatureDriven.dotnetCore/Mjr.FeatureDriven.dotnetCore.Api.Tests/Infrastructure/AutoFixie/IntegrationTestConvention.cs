using Ploeh.AutoFixture;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
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