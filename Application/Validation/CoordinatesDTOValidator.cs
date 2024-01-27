using Domain.DTO;
using FluentValidation;

namespace Application.Validation;

public class CoordinatesDTOValidator : AbstractValidator<CoordinatesDTO>
{
    private const string NOT_FOUND = "Incorrect address, coordinates not found.";

    public CoordinatesDTOValidator()
    {
        RuleFor(coords => coords.Latitude)
            .Cascade(CascadeMode.Stop)
            .NotEqual(double.MinValue)
                .WithMessage(NOT_FOUND)
            .NotNull()
            .InclusiveBetween(-90d, 90d);

        RuleFor(coords => coords.Longtitude)
            .Cascade(CascadeMode.Stop)
            .NotEqual(double.MinValue)
                .WithMessage(NOT_FOUND)
            .NotNull()
            .InclusiveBetween(-180d, 180d);
    }
}
