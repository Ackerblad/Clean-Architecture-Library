using Application.DTOs.UserDtos;
using Domain.Results;
using MediatR;

namespace Application.Commands.Users.CreateUser
{
    public class CreateUserCommand : IRequest<OperationResult<UserDto>>
    {
        public CreateUserDto NewUser { get; set; }

        public CreateUserCommand(CreateUserDto newUser)
        {
            NewUser = newUser;
        }
    }
}
