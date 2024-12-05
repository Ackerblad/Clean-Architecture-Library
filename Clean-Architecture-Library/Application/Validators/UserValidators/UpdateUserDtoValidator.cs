﻿using Application.DTOs.UserDtos;
using FluentValidation;

namespace Application.Validators.UserValidators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
