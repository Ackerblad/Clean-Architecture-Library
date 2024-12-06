using Domain.Results;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQuery : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginUserQuery(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
