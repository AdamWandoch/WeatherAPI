using Common.Utilities;
using Domain.DTO;
using FluentValidation;

namespace Application.Validation;

public class AddressDTOValidator : AbstractValidator<AddressDTO>
{
    public AddressDTOValidator()
    {
        RuleFor(address => address.Number)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(address => address.Street)
            .NotEmpty();

        RuleFor(address => address.City)
            .NotEmpty();

        RuleFor(address => address.State)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(2)
                .WithMessage("State should consist of 2 letters only (state abbreviation). For example: AL for Alabama state.")
            .Must(state => StringCollections.UsStates.Contains(state))
                .WithMessage("State abrreviation invalid.");

        RuleFor(address => address.ZipCode)
            .NotEmpty()
            .GreaterThan(0);
    }
}
