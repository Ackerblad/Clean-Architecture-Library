using Domain.Entities;
using MediatR;

namespace Application.Commands.Users.CreateUser
{
    public class CreateUserCommand : IRequest<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public CreateUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
