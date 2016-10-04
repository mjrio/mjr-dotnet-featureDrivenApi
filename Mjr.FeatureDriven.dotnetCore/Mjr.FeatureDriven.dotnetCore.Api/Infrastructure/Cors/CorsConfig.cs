using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Cors
{
    public static class CorsConfig
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
                return services;
            return services.AddCors();
            
        }
        public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
                return app;

            return app.UseCors(builder =>
             {
                 builder.AllowAnyHeader();
                 builder.WithMethods("GET", "POST");
                 builder.WithOrigins("http://localhost:9001", "http://localhost:9000");
             });
        }
    }
}
