using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public abstract class AutoFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            //find all ISpecimenBuilders in this assembly and add them to the fixture customization
            var propertyBuilders = typeof(AutoFixtureCustomization)
                .Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(ISpecimenBuilder).IsAssignableFrom(t))
                .Select(Activator.CreateInstance)
                .Cast<ISpecimenBuilder>();

            foreach (var propertyBuilder in propertyBuilders)
            {
                fixture.Customizations.Add(propertyBuilder);
            }
            
            CustomizeFixture(fixture);

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        
        protected abstract void CustomizeFixture(IFixture fixture);
    }
}