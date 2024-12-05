using Application.DTOs.AuthorDtos;
using Domain.Results;
using MediatR;

namespace Application.Commands.Authors.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<OperationResult<AuthorDto>>
    {
        public CreateAuthorDto NewAuthor { get; set; }

        public CreateAuthorCommand(CreateAuthorDto newAuthor)
        {
            NewAuthor = newAuthor;
        }
    }
}
