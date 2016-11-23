using System;
using Mjr.FeatureDriven.net4.Api.Data;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Data;
using Mjr.FeatureDriven.net4.Api.Infrastructure.IoC;
using Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Security;
using StructureMap;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
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
                    cfg.For<BaseDbContext>().Use<ApiContext>();
                    cfg.For<IUserSession>().Use(new TestUserSession()).Transient();
                });
                Container = container;
            }
            public IContainer Container { get; }
        }
    }
}