using FluentValidation;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Validation
{
    public class NumberShouldBeGreaterThanValidator : AbstractValidator<int>
    {
        public NumberShouldBeGreaterThanValidator(int number)
        {
            RuleFor(p => p).GreaterThan(number);
        }
    }
}