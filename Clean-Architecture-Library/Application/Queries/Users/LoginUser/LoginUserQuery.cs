using Domain.Results;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQuery : IRequest<OperationResult<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
