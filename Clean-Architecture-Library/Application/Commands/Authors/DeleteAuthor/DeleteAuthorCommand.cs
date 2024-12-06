using Domain.Results;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<bool>>
    {
        public Guid AuthorId { get; set; }

        public DeleteAuthorCommand(Guid authorId)
        {
            AuthorId = authorId;
        }
    }
}
