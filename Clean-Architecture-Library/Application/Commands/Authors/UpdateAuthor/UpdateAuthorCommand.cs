using Application.DTOs.AuthorDtos;
using Domain.Results;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<OperationResult<AuthorDto>>
    {
        public Guid AuthorId { get; set; }
        public UpdateAuthorDto UpdatedAuthor { get; set; }

        public UpdateAuthorCommand(Guid authorId, UpdateAuthorDto updatedAuthor)
        {
            AuthorId = authorId;
            UpdatedAuthor = updatedAuthor;
        }
    }
}
