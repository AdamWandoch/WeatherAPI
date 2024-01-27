using Domain.DTO;
using FluentValidation;

namespace Application.Validation;

public class AddressDTOValidator : AbstractValidator<AddressDTO>
{
    public AddressDTOValidator()
    {
        RuleFor(address => address.Number)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(address => address.Street)
            .NotNull()
            .NotEmpty();

        RuleFor(address => address.City)
            .NotNull()
            .NotEmpty();

        RuleFor(address => address.State)
            .NotNull()
            .NotEmpty()
            .Length(2)
                .WithMessage("State should consist of 2 letters only (state abbreviation). For example: AL for Alabama state.");

        RuleFor(address => address.ZipCode)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}
