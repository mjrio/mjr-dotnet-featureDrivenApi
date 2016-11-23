using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Routing
{
    /// <summary>
    ///     Controller selector implementation that uses the controller namespace
    ///     as part of the selection decision.
    /// </summary>
    /// <remarks>
    ///     Largely inspired by sample located at:
    ///     http://blogs.msdn.com/b/webdev/archive/2013/03/08/using-namespaces-to-version-web-apis.aspx
    /// </remarks>
    public class NamespaceHttpControllerSelector : IHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;

        public NamespaceHttpControllerSelector(HttpConfiguration config)
        {
            _configuration = config;
            _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerName = GetControllerName(routeData);
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //could work with route variables, but didn't find a way to make it work with swashbuckle
            var api = request.RequestUri.Segments.ToList().IndexOf("api/");
            var namespaceName = request.RequestUri.Segments[api + 1].Replace("/", "");//GetVersion(routeData);
            if (namespaceName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerKey = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, controllerName);

            HttpControllerDescriptor controllerDescriptor;
            foreach (var httpControllerDescriptor in _controllers.Value)
            {
                if (httpControllerDescriptor.Key.StartsWith(namespaceName,StringComparison.InvariantCultureIgnoreCase))
                {
                    if (httpControllerDescriptor.Key.EndsWith(controllerName, StringComparison.InvariantCultureIgnoreCase))
                        return httpControllerDescriptor.Value;
                }
            }
            if (_controllers.Value.TryGetValue(controllerKey, out controllerDescriptor))
            {
                return controllerDescriptor;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllers.Value;
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            var assembliesResolver = _configuration.Services.GetAssembliesResolver();
            var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (var controllerType in controllerTypes)
            {
                var segments = controllerType.Namespace.Split(Type.Delimiter);

                var controllerName =
                    controllerType.Name.Remove(controllerType.Name.Length -
                                               DefaultHttpControllerSelector.ControllerSuffix.Length);
                var includeInKey = false;
                var controllerKey = string.Empty;
                foreach (var segment in segments)
                {
                    if (!includeInKey)
                    {
                        if (segment.Equals("Features"))
                            includeInKey = true;
                    }
                    else
                    {
                        controllerKey += $"{segment}.";
                    }
                }
                controllerKey += controllerName;

                if (!dictionary.Keys.Contains(controllerKey))
                {
                    var descriptor=new HttpControllerDescriptor(_configuration, controllerType.Name,
                        controllerType);
                    dictionary[controllerKey] = descriptor;
                }
            }

            return dictionary;
        }

        private T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }

        private string GetControllerName(IHttpRouteData routeData)
        {
            var subroute = routeData.GetSubRoutes().FirstOrDefault();

            var dataTokenValue = subroute?.Route.DataTokens.First().Value;

            var controllerName =
                ((HttpActionDescriptor[]) dataTokenValue)?.First().ControllerDescriptor.ControllerName.Replace("Controller", string.Empty);
            return controllerName;
        }

        private string GetVersion(IHttpRouteData routeData)
        {
            var subRouteData = routeData.GetSubRoutes().FirstOrDefault();
            if (subRouteData == null) return null;
            return GetRouteVariable<string>(subRouteData, "apiVersion");
        }
    }
}