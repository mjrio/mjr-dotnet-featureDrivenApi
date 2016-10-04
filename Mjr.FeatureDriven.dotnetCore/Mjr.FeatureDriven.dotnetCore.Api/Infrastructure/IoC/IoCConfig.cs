using StructureMap;
using System;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mapping;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.IoC
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
    }
}
