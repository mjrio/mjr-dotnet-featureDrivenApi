using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;
using System.IO;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Swagger
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.MultipleApiVersions(
                    new[]
                    {
                        new Info { Version = "v0", Title = "API V0" },
                        new Info { Version = "v1", Title = "API V1" },
                        new Info { Version = "v2", Title = "API V2" }
                    },
                    ResolveVersionSupportByVersionsConstraint

                );
                //c.SingleApiVersion(new Info
                //{
                //    Version = "v1",
                //    Title = "Mjr.FeatureDriven.dotnetCore api",
                //    Description = "Interactive documentation",
                //});
            });

            services.ConfigureSwaggerGen(c =>
            {
                c.CustomSchemaIds((t) => t.FullName);
                c.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));
            });

            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger((httpRequest, swaggerDoc) =>
            {
                swaggerDoc.Host = httpRequest.Host.Value;

            });

            app.UseSwaggerUi();

            return app;
        }

        private static bool ResolveVersionSupportByVersionsConstraint(ApiDescription apiDesc, string version)
        {
            return apiDesc.RelativePath.Contains(version);
        }
        private static string GetXmlCommentsPath(ApplicationEnvironment appEnvironment)
        {
            return Path.Combine(appEnvironment.ApplicationBasePath, "Mjr.FeatureDriven.dotnetCore.Api.xml");

        }
    }
}
