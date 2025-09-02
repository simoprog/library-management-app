using FluentValidation;

namespace LibraryManagement.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Author is required")
            .MaximumLength(100)
            .WithMessage("Author cannot exceed 100 characters");

        RuleFor(x => x.ISBN)
            .NotEmpty()
            .WithMessage("ISBN is required")
            .Matches(@"^\d{10}(\d{3})?$")
            .WithMessage("ISBN must be 10 or 13 digits");
    }
}
