using Domain.Entities;
using MediatR;

namespace Application.Commands.Authors.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<Author>
    {
        public string FirstName { get; }
        public string LastName { get; }

        public CreateAuthorCommand(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
