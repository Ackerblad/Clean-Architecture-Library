using Application.DTOs.BookDtos;
using Domain.Results;
using MediatR;

namespace Application.Queries.Books.GetBookById
{
    public class GetBookByIdQuery : IRequest<OperationResult<BookDto>>
    {
        public Guid BookId { get; set; }

        public GetBookByIdQuery(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
