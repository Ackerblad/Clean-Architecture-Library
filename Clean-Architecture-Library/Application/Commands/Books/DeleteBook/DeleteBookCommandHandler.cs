using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<bool>>
    {
        private readonly ICommandRepository<Book> _commandRepository;
        private readonly IQueryRepository<Book> _queryRepository;
        private readonly ILogger<DeleteBookCommandHandler> _logger;

        public DeleteBookCommandHandler(ICommandRepository<Book> commandRepository, IQueryRepository<Book> queryRepository, ILogger<DeleteBookCommandHandler> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteBookCommand for BookId: {BookId}", request.BookId);

            var bookExists = await _queryRepository.GetByIdAsync(request.BookId);
            if (bookExists == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found.", request.BookId);
                return OperationResult<bool>.Failure($"Book with ID {request.BookId} was not found.", "Error: Book not found.");
            }

            await _commandRepository.DeleteAsync(request.BookId);
            _logger.LogInformation("Book with ID {BookId} deleted successfully.", request.BookId);

            return OperationResult<bool>.Successful(true, "Book deleted successfully.");
        }
    }
}
