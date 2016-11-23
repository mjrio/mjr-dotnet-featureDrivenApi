using Newtonsoft.Json.Converters;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.JsonConverters
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}