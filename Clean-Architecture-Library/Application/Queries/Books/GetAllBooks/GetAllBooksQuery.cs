using Domain.Entities;
using MediatR;

namespace Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<List<Book>> { }
}
