using AutoMapper;
using MediatR;
using StructureMap;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.IoC
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.LookForRegistries();
                scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                scanner.AddAllTypesOf(typeof(INotificationHandler<>));
                scanner.AddAllTypesOf(typeof(IAsyncRequestHandler<,>));
                scanner.AddAllTypesOf(typeof(IAsyncNotificationHandler<>));
                scanner.AddAllTypesOf<Profile>();
            }
            );
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
    }
}

