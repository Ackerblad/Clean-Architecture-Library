using Application.DTOs.BookDtos;
using FluentValidation;

namespace Application.Validators.BookValidators
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
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
