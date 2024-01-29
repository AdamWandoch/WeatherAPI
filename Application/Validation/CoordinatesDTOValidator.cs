using Domain.DTO;
using FluentValidation;

namespace Application.Validation;

public class CoordinatesDTOValidator : AbstractValidator<CoordinatesDTO>
{
    private const string NotFound = "not found.";

    public CoordinatesDTOValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(coords => coords.Latitude)
            .Cascade(CascadeMode.Stop)
            .NotEqual(double.MinValue)
                .WithMessage(NotFound)
            .NotNull()
            .InclusiveBetween(-90d, 90d);

        RuleFor(coords => coords.Longtitude)
            .Cascade(CascadeMode.Stop)
            .NotEqual(double.MinValue)
                .WithMessage(NotFound)
            .NotNull()
            .InclusiveBetween(-180d, 180d);
    }
}
