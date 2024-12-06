using Domain.Results;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<bool>>
    {
        public Guid BookId { get; set; }

        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
