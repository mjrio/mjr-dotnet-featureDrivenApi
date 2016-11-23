using Mjr.FeatureDriven.net4.Api.Infrastructure.IoC;
using Ploeh.AutoFixture;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public class IntegrationTestsFixtureCustomization : AutoFixtureCustomization
    {
        protected override void CustomizeFixture(IFixture fixture)
        {
            var scope = new StructureMapDependencyResolver(IntegrationTestContainerFactory.Container);
            var contextFixture = new TestContextFixture(scope, fixture);
            contextFixture.SetUp();

            fixture.Register(() => contextFixture);
        }
    }
}