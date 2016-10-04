namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
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