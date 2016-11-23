using System.Web.Http;
using StructureMap;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing
{
    public class TracingConfig
    {
        public static void Configure(HttpConfiguration config, IContainer container)
        {
            var tracing = config.EnableSystemDiagnosticsTracing();
            tracing.MinimumLevel = System.Web.Http.Tracing.TraceLevel.Debug;
            tracing.TraceSource = ApiEventSource.TraceSource;

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new WebApiTraceWriter());

            var messageHandler = container.GetInstance<MessageBodyHandler>();
            config.MessageHandlers.Add(messageHandler);
        }
    }
}