using System;
using System.Collections.Concurrent;
using Ploeh.AutoFixture;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public class AutoFixtureFactory
    {
        private static readonly Lazy<AutoFixtureFactory> Factory = new Lazy<AutoFixtureFactory>(() => new AutoFixtureFactory());

        private readonly ConcurrentDictionary<Type, IFixture> _autoFixtureCache;

        private AutoFixtureFactory()
        {
            _autoFixtureCache = new ConcurrentDictionary<Type, IFixture>();
        }

        public static AutoFixtureFactory Instance => Factory.Value;

        public IFixture BuildWith(ICustomization autoFixtureCustomization)
        {
            return _autoFixtureCache.GetOrAdd(autoFixtureCustomization.GetType(),
                type => new Fixture().Customize(autoFixtureCustomization));
        }
    }
}