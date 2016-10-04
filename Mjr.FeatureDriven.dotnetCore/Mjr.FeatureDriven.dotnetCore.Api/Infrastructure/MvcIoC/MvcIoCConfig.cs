using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using System;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.MvcIoC
{
    public static class MvcIoCConfig
    {
        public static IServiceProvider ConfigureMvcIoC(this IServiceCollection services)
        {
            var container = IoCConfig.Container;
            services.AddSingleton<IControllerActivator > (
             new StructureMapControllerActivator(IoCConfig.Container));
            container.Configure(config =>
            {
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public static IApplicationBuilder ConfigureMvcIoC(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                var logger = loggerFactory.CreateLogger("IoC.Diagnostics");
                logger.LogTrace(IoCConfig.Container.WhatDidIScan());
                logger.LogTrace(IoCConfig.Container.WhatDoIHave(assembly: typeof(Startup).Assembly));
                logger.LogTrace(IoCConfig.Container.WhatDoIHave(assembly: typeof(AutoMapper.IMapper).Assembly));
                logger.LogTrace(IoCConfig.Container.WhatDoIHave());
            }
            return app;
        }
    }
}
