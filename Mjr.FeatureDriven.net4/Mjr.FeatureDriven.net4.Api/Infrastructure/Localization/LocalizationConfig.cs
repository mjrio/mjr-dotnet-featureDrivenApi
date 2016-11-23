using System.Web.Http;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Localization
{

    public static class LocalizationConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new LocalizationMessageHandler());
        }
    }
}