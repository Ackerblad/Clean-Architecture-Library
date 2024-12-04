using Application.DTOs.BookDtos;
using FluentValidation;

namespace Application.Validators
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(book => book.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(book => book.AuthorId)
                .NotEmpty().WithMessage("Author ID is required.");
        }
    }
}
