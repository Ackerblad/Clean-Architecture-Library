using Application.DTOs.BookDtos;
using Domain.Results;
using MediatR;

namespace Application.Commands.Books.CreateBook
{
    public class CreateBookCommand : IRequest<OperationResult<BookDto>>
    {
        public CreateBookDto NewBook { get; set; }

        public CreateBookCommand(CreateBookDto newBook)
        {
            NewBook = newBook;
        }
    }
}
