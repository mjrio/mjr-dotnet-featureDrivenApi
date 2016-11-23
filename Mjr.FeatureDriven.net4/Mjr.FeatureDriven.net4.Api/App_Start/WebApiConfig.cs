using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication;
using Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling;
using Mjr.FeatureDriven.net4.Api.Infrastructure.IoC;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Localization;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Logging;
using Mjr.FeatureDriven.net4.Api.Infrastructure.MediaTypeFormatting;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Routing;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Validation;

namespace Mjr.FeatureDriven.net4.Api.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            MediaTypeFormattingConfig.Configure(config);
            RoutingConfig.Configure(config);
            IoCConfig.Configure(config);
            TracingConfig.Configure(config, IoCConfig.Container);
            LoggingConfig.Configure();
            ExceptionHandlingConfig.Configure(config);
            ValidationConfig.Configure(config);
            LocalizationConfig.Configure(config);

            //configure authentication
            config.MessageHandlers.Insert(0, new AuthenticationHandler());

            //for gzip compression use:
           // config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));

        }
    }
}