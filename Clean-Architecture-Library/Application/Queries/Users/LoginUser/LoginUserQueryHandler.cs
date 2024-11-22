using Application.Queries.Users.LoginUser.Helpers;
using Infrastructure;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly FakeDatabase _fakeDatabase;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(FakeDatabase fakeDatabase, TokenHelper tokenHelper)
        {
            _fakeDatabase = fakeDatabase;
            _tokenHelper = tokenHelper;
        }

        public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = _fakeDatabase.Users.FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            string token = _tokenHelper.GenerateJwtToken(user);

            return Task.FromResult(token);
        }
    }
}
