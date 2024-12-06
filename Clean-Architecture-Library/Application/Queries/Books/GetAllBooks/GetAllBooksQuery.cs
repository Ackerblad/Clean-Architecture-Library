using Application.DTOs.BookDtos;
using MediatR;

namespace Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>{}
}
