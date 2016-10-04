using System;
using System.Collections.Generic;
using System.Reflection;
using Fixie;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public abstract class FixieConvention : Convention
    {
        public FixieConvention()
        {
            Classes
                .NameEndsWith("Tests");
            
            ClassExecution
                .UsingFactory(CustomFactory);

            Parameters
                .Add(GetParameters);
        }

        protected abstract ICustomization AutoFixtureCustomization { get; }

        protected IFixture AutoFixture => AutoFixtureFactory.Instance.BuildWith(AutoFixtureCustomization);

        private IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            return method.ResolveParametersWith(AutoFixture);
        }

        private object CustomFactory(Type t)
        {
            return new SpecimenContext(AutoFixture).Resolve(t);
        }
    }
}