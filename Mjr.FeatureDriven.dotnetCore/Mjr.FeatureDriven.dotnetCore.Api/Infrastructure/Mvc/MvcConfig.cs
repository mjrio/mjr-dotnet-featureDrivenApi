using Microsoft.Extensions.DependencyInjection;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc.ActionFilters;
using Microsoft.AspNetCore.Builder;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc.ExceptionFilters;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc
{
    public static class MvcConfig
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationFilter));
                options.Filters.Add(typeof(DbUpdateConcurrencyExceptionFilter), 1);
                options.Filters.Add(typeof(GlobalExceptionFilter), 0);
            }

           )
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            })
            .AddControllersAsServices();

            return services;
        }


        public static IApplicationBuilder ConfigureMvc(this IApplicationBuilder app)
        {
            app.UseMvc();
            return app;
        }
    }
}
