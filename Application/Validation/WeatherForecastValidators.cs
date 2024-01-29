using Common.Utilities;
using Domain.DTO;
using FluentValidation;

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
            .Must(date => date.Date >= DateTime.Today.AddDays(-1))
                .WithMessage("The start time must be yesterday, today or in the future.");

        RuleFor(period => period.EndTime)
            .Must(date => date.Date >= DateTime.Today)
                .WithMessage("The end time must be today or in the future.");

        RuleFor(period => period.IsDaytime)
            .NotNull();

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

        RuleFor(period => period.WindSpeed)
            .NotNull()
            .NotEmpty();

        RuleFor(period => period.WindDirection)
            .NotNull()
            .NotEmpty()
            .Must(direction => StringCollections.WindDirections.Contains(direction.ToUpper()))
                .WithMessage($"The wind direction value has to match one of the following: " +
                             $"{string.Join(" ,", StringCollections.WindDirections)}.");

        RuleFor(period => period.ShortForecast)
            .NotNull()
            .NotEmpty();

        RuleFor(period => period.DetailedForecast)
            .NotNull()
            .NotEmpty();

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
            .NotEmpty()
            .Must(value => value >= 0 && value <= 100)
                .WithMessage("The realtive humidity must be between 0% and 100%");
    }
}
