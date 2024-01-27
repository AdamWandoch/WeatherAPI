using Common.Utilities;
using FluentValidation;

namespace Application.Validation;

public class ForecastURLValidator : AbstractValidator<string>
{
    public ForecastURLValidator()
    {
        RuleFor(forecast => forecast)
            .NotNull()
            .NotEmpty()
            .Matches(RegexPatterns.URL)
                .WithMessage("The returned URL is not valid.")
            .WithName("ForecastURL");
    }
}
