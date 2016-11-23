using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation.Validators;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Validation
{
    public class StringShouldContainOnlyNumbersValidator : PropertyValidator
    {

        public StringShouldContainOnlyNumbersValidator()
            : base("Property {PropertyName} is not a number!")
        {

        }

        public static bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            return value.ToCharArray().Reverse().All(char.IsDigit);
        }

        [ExcludeFromCodeCoverage]
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;
            return IsValid(value);
        }
    }
}