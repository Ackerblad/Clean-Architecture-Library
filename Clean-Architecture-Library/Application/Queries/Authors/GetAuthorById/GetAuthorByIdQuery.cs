using Domain.Entities;
using MediatR;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<Author>
    {
        public int Id { get; set; }

        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
