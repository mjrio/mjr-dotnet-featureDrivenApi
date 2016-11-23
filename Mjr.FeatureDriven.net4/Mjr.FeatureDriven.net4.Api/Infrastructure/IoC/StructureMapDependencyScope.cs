using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.IoC
{
    /// <summary>
    /// http://benfoster.io/blog/per-request-dependencies-in-aspnet-web-api-using-structuremap
    /// </summary>
    public class StructureMapDependencyScope : IDependencyScope//, IServiceProvider
    {
        private IContainer _container;

        public StructureMapDependencyScope(IContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }

        public object GetService(Type serviceType)
        {
            return serviceType.IsAbstract || serviceType.IsInterface
                ? _container.TryGetInstance(serviceType)
                : _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}