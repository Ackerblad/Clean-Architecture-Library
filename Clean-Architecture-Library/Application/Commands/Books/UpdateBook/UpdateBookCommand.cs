using Application.DTOs.BookDtos;
using Domain.Results;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookCommand : IRequest<OperationResult<BookDto>>
    {
        public Guid BookId { get; set; }
        public UpdateBookDto UpdatedBook { get; set; }

        public UpdateBookCommand(Guid bookId, UpdateBookDto updatedBook)
        {
            BookId = bookId;
            UpdatedBook = updatedBook;
        }
    }
}
