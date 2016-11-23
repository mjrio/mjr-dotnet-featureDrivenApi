using AutoMapper;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Mapping
{
    public class AutoMapperTests
    {
        public void ShouldHaveValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
