using Application.DTOs.AuthorDtos;
using FluentValidation;

namespace Application.Validators
{
    public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorDtoValidator()
        {
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last name is required.");
        }
    }
}
