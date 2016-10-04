using Microsoft.AspNetCore.Mvc;
using StructureMap;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.MvcIoC
{
    /// <summary>
    /// If the built in IoC container does not suffice, could use this code to configure the services
    /// services.AddSingletonIControllerActivator>(
    /// new StructureMapControllerActivator(IoCConfig.Container));
    /// </summary>
    public class StructureMapControllerActivator : Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator
    {
        private readonly IContainer container;
        public StructureMapControllerActivator(IContainer container)
        {
            this.container = container;
        }

        public object Create(ControllerContext context)
        {
            return container.GetInstance(context.ActionDescriptor.ControllerTypeInfo.AsType());
        }

        public void Release(ControllerContext context, object controller)
        {
            
        }
    }

}