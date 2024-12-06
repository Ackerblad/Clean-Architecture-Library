using Application.Interfaces.HelperInterfaces;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.LoginUser.Helpers;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IQueryRepository<User> _userRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly ILogger<LoginUserQueryHandler> _logger;

        public LoginUserQueryHandler(IQueryRepository<User> userRepository, ITokenHelper tokenHelper, ILogger<LoginUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LoginUserQuery for username: {Username}", request.Username);

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                _logger.LogWarning("Invalid username or password for username: {Username}", request.Username);
                return OperationResult<string>.Failure("Invalid username or password.", "Unauthorized");
            }

            var token = _tokenHelper.GenerateJwtToken(user);

            _logger.LogInformation("User {Username} successfully logged in.", request.Username);
            return OperationResult<string>.Successful(token, "Login successful.");
        }
    }
}
