using System;
using System.Web.Http;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Mapping;
using StructureMap;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.IoC
{
    public static class IoCConfig
    {
        private static readonly Lazy<IContainer> Bootstrapper = new Lazy<IContainer>(Initialize, true);
        public static IContainer Container => Bootstrapper.Value;

        private static IContainer Initialize()
        {
            var container = new Container(new DefaultRegistry());
            MappingConfig.Configure(container);
            return container;
        }
        public static void Configure(HttpConfiguration config)
        {
            config.DependencyResolver = new StructureMapDependencyResolver(Container);
        }
    }
}