using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<bool>>
    {
        private readonly ICommandRepository<Author> _commandRepository;
        private readonly IQueryRepository<Author> _queryRepository;
        private readonly IQueryRepository<Book> _bookRepository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(ICommandRepository<Author> commandRepository, IQueryRepository<Author> queryRepository,
                                          IQueryRepository<Book> bookRepository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteAuthorCommand for AuthorId: {AuthorId}", request.AuthorId);

            var authorExists = await _queryRepository.GetByIdAsync(request.AuthorId);
            if (authorExists == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", request.AuthorId);
                return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} was not found.", "Error: Author not found.");
            }

            var associatedBooks = (await _bookRepository.GetAllAsync()).Any(book => book.AuthorId == request.AuthorId);
            if (associatedBooks)
            {
                _logger.LogWarning("Cannot delete author with ID {AuthorId} because they are associated with existing books.", request.AuthorId);
                return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} cannot be deleted as they are associated with books.", "Error: Author has associated books.");
            }

            await _commandRepository.DeleteAsync(request.AuthorId);
            _logger.LogInformation("Author with ID {AuthorId} deleted successfully.", request.AuthorId);

            return OperationResult<bool>.Successful(true, "Author deleted successfully.");
        }
    }
}
