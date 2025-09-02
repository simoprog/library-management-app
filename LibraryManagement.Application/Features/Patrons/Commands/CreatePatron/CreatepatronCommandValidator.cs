using FluentValidation;

namespace LibraryManagement.Application.Features.Patrons.Commands.CreatePatron;

public class CreatePatronCommandValidator : AbstractValidator<CreatePatronCommand>
{
    public CreatePatronCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Patron type is required")
            .Must(x => x.Equals("Regular", StringComparison.OrdinalIgnoreCase) || 
                       x.Equals("Researcher", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Patron type must be 'Regular' or 'Researcher'");
    }
}