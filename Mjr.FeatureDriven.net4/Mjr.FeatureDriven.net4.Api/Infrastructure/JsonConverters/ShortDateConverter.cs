using Newtonsoft.Json.Converters;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.JsonConverters
{
    public class ShortDateConverter : IsoDateTimeConverter
    {
        public ShortDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}