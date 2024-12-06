using Application.DTOs.AuthorDtos;
using MediatR;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>{}
}
