using Application.DTOs.AuthorDtos;
using Domain.Results;
using MediatR;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<OperationResult<AuthorDto>>
    {
        public Guid AuthorId { get; set; }

        public GetAuthorByIdQuery(Guid authorId)
        {
            AuthorId = authorId;
        }
    }
}
