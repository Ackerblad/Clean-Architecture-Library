using Domain.Entities;
using Infrastructure;
using MediatR;

namespace Application.Commands.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly FakeDatabase _fakeDatabase;

        public CreateUserCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_fakeDatabase.Users.Any(u => u.UserName == request.UserName))
            {
                throw new InvalidOperationException($"The username {request.UserName} is not available.");
            }

            var newUser = new User(
               _fakeDatabase.Users.Count + 1,
               request.UserName,
               request.Password
           );

            _fakeDatabase.Users.Add(newUser);

            return Task.FromResult(newUser);
        }
    }
}
