using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Swagger;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Cors;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Routing;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Database;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.MvcIoC;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Logging;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.ExceptionHandling;

namespace Mjr.FeatureDriven.dotnetCore.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        public Startup(IHostingEnvironment env)
        {
            _hostingEnv = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services
                .ConfigureSwagger()
                .ConfigureCors(_hostingEnv)
                .ConfigureRouting()
                .ConfigureDatabase(Configuration.GetConnectionString("SqlDatabase"))
                .ConfigureMvc()
                .ConfigureMvcIoC()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .ConfigureLogging(env, Configuration.GetSection("Logging"));

            app
                .ConfigureExceptionHandling(env)
                .ConfigureCors(env)
                .ConfigureRouting()
                .ConfigureMvc()
                .ConfigureSwagger()
                .ConfigureMvcIoC(env, loggerFactory);

            app.UseStaticFiles();
        }
    }
}
