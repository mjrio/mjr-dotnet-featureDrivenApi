using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Mapping
{
    public class AutoMapperTests
    {
        public void ShouldHaveValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
