using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Routing
{
    public static class RoutingConfig
    {
       
        public static IServiceCollection ConfigureRouting(this IServiceCollection services)
        {
            services.AddRouting();
            return services;
        }

        public static IApplicationBuilder ConfigureRouting(this IApplicationBuilder app)
        {
            var endpoint1 = new RouteHandler((c) =>
            {
                return c.Response.WriteAsync($"match2, route values - {string.Join(", ", c.GetRouteData().Values)}");
            });

            var endpoint2 = new RouteHandler((c) => c.Response.WriteAsync("Hello, World!"));

            var routeBuilder = new RouteBuilder(app)
            {
                DefaultHandler = endpoint1,
            };

            routeBuilder.MapRoute("api/status/{item}", c => c.Response.WriteAsync($"{c.GetRouteValue("item")} is just fine."));
            
            routeBuilder.AddPrefixRoute("api/store", endpoint1);
            routeBuilder.AddPrefixRoute("hello/world", endpoint2);

            routeBuilder.MapLocaleRoute("en-US", "store/US/{action}", new { controller = "Store" });
            routeBuilder.MapLocaleRoute("en-GB", "store/UK/{action}", new { controller = "Store" });

            app.UseRouter(routeBuilder.Build());
            return app;
        }
        
    }
}
