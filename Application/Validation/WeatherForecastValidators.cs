using Common.Utilities;
using Domain.DTO;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Validation;

public class WeatherForecastDTOValidator : AbstractValidator<WeatherForecastDTO>
{
    public WeatherForecastDTOValidator()
    {
        RuleFor(forecast => forecast.Periods)
            .NotNull()
            .Must(x => x?.Count > 0)
                .WithMessage("Forecast returned 0 results.");

        RuleForEach(forecast => forecast.Periods)
            .SetValidator(new PeriodDTOValidator());
    }
}

public class PeriodDTOValidator : AbstractValidator<PeriodDTO>
{
    public PeriodDTOValidator()
    {
        RuleFor(period => period.Number)
            .NotNull()
            .NotEmpty()
            .InclusiveBetween(1, 14);

        RuleFor(period => period.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(period => period.StartTime)
            .Must(date => date.Date >= DateTime.Today)
                .WithMessage("The start time must be today or in the future.");

        RuleFor(period => period.EndTime)
            .Must(date => date.Date >= DateTime.Today)
                .WithMessage("The end time must be today or in the future.");

        RuleFor(period => period.IsDayTime)
            .NotNull()
            .NotEmpty();

        RuleFor(period => period.Temperature)
            .NotNull()
            .NotEmpty();

        RuleFor(period => period.TemperatureUnit)
            .NotNull()
            .NotEmpty()
            .Must(symbol => symbol == 'F' || symbol == 'C' || symbol == 'K')
                .WithMessage("The temperature unit has to either 'F' (Fahrenheit), 'C' (Celsius) or 'K' (Kelvin)");

        RuleFor(period => period.RelativeHumidity)
            .SetValidator(new HumidityDTOValidator());


        RuleFor(period => period.Icon)
            .NotNull()
            .NotEmpty()
            .Matches(RegexPatterns.URL)
                .WithMessage("The icon url is not valid");
    }
}

public class HumidityDTOValidator : AbstractValidator<HumidityDTO>
{
    public HumidityDTOValidator()
    {
        RuleFor(humidity => humidity.Value)
            .NotNull()
            .NotEmpty();

    }
}