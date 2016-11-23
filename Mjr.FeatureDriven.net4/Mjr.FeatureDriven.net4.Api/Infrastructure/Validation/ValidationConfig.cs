using System.Web.Http;
using FluentValidation.WebApi;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Validation
{
    public static class ValidationConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            FluentValidationModelValidatorProvider.Configure(config);
            config.Filters.Add(new ValidationFilter());
        }
    }
}