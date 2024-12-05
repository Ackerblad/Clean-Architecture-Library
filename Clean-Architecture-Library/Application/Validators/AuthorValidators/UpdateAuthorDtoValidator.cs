using Application.DTOs.AuthorDtos;
using FluentValidation;

namespace Application.Validators.AuthorValidators
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last name is required.");
        }
    }
}
