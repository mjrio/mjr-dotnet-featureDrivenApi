using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.IoC;
using Ploeh.AutoFixture;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public class IntegrationTestsFixtureCustomization : AutoFixtureCustomization
    {
        protected override void CustomizeFixture(IFixture fixture)
        {
            var scope = IntegrationTestContainerFactory.Container;
            var contextFixture = new TestContextFixture(scope, fixture);
            contextFixture.SetUp();

            fixture.Register(() => contextFixture);
        }
    }
}