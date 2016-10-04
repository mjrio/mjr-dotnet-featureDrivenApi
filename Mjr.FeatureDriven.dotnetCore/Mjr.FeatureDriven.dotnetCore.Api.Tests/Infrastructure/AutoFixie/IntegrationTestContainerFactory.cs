using System;
using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.IoC;
using StructureMap;
using Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Data;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public class IntegrationTestContainerFactory
    {
        private static readonly Lazy<InnerFactory> LazyInnerFactory = new Lazy<InnerFactory>(() => new InnerFactory());

        public static IContainer Container => LazyInnerFactory.Value.Container;

        private class InnerFactory
        {
            public InnerFactory()
            {
                var container = IoCConfig.Container;
                container.Configure(cfg =>
                {
                    cfg.For<Context>().Use<UnitTestContext>().Transient();
                });
                Container = container;
            }
            public IContainer Container { get; }
        }
    }
}