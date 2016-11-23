using System.Web.Http;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Routing
{
    public class ApiVersion1RoutePrefixAttribute : RoutePrefixAttribute
    {
        private const string RouteBase = "api/v1";
        private const string PrefixRouteBase = RouteBase + "/";

        public ApiVersion1RoutePrefixAttribute(string routePrefix)
            : base(string.IsNullOrWhiteSpace(routePrefix) ? RouteBase : PrefixRouteBase + routePrefix)
        {
        }
    }
    public class ApiVersion2RoutePrefixAttribute : RoutePrefixAttribute
    {
        private const string RouteBase = "api/v2";
        private const string PrefixRouteBase = RouteBase + "/";

        public ApiVersion2RoutePrefixAttribute(string routePrefix)
            : base(string.IsNullOrWhiteSpace(routePrefix) ? RouteBase : PrefixRouteBase + routePrefix)
        {
        }
    }
}