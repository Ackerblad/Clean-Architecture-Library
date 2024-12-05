using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<BookDto>>
    {
        private readonly ICommandRepository<Book> _commandRepository;
        private readonly IQueryRepository<Book> _queryRepository;
        private readonly IQueryRepository<Author> _authorRepository;
        private readonly IValidator<UpdateBookDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBookCommandHandler> _logger;

        public UpdateBookCommandHandler(ICommandRepository<Book> commandRepository, IQueryRepository<Book> queryRepository,
                                        IQueryRepository<Author> authorRepository, IValidator<UpdateBookDto> validator, 
                                        IMapper mapper, ILogger<UpdateBookCommandHandler> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _authorRepository = authorRepository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateBookCommand for BookId: {BookId}", request.BookId);

            var validationResult = await _validator.ValidateAsync(request.UpdatedBook, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                return OperationResult<BookDto>.Failure(errors, "Validation failed.");
            }

            var existingBook = await _queryRepository.GetByIdAsync(request.BookId);
            if (existingBook == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found.", request.BookId);
                return OperationResult<BookDto>.Failure($"Book with ID {request.BookId} was not found.", "Error: Book not found.");
            }

            var authorExists = await _authorRepository.GetByIdAsync(request.UpdatedBook.AuthorId);
            if (authorExists == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", request.UpdatedBook.AuthorId);
                return OperationResult<BookDto>.Failure($"Author with ID {request.UpdatedBook.AuthorId} was not found.", "Error: Author not found.");
            }

            _mapper.Map(request.UpdatedBook, existingBook);
            await _commandRepository.UpdateAsync(existingBook);
            _logger.LogInformation("Book with ID {BookId} updated successfully.", request.BookId);

            var updatedBookDto = _mapper.Map<BookDto>(existingBook);
            return OperationResult<BookDto>.Successful(updatedBookDto, "Book updated successfully.");
        }
    }
}
