namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public class FixturePerMethodConvention : IntegrationTestConvention
    {
        public FixturePerMethodConvention()
        {
            Classes
                .ConstructorDoesntHaveArguments();
            ClassExecution
                .CreateInstancePerCase();

            FixtureExecution
                .Wrap<ClearPermissions>()
                .Wrap<DeleteData>();

        }
    }
}