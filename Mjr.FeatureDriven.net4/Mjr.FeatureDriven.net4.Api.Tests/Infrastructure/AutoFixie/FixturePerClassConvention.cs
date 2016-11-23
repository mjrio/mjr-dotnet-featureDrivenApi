namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public class FixturePerClassConvention : IntegrationTestConvention
    {
        public FixturePerClassConvention()
        {
            Classes
                .ConstructorHasArguments();

            
            ClassExecution
                .CreateInstancePerClass()
                .Wrap<DeleteData>();
        }
    }
}