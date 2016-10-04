using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie.SpecimenBuilders
{
  
   
    public class IdOmitterBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var property = request as PropertyInfo;

            if (property == null)
            {
                return new NoSpecimen();
            }

            if (IsId(property))
            {
                return Activator.CreateInstance(property.PropertyType);
            }

            return new NoSpecimen();
        }

        private static bool IsId(PropertyInfo property)
        {
            return property.Name.EndsWith("Id") && (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?));
        }
    }
}