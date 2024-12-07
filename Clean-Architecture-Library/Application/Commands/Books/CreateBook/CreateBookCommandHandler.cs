using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<BookDto>>
    {
        private readonly ICommandRepository<Book> _bookRepository;
        private readonly IQueryRepository<Author> _authorRepository;
        private readonly IValidator<CreateBookDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(ICommandRepository<Book> bookRepository, IQueryRepository<Author> authorRepository, 
                                        IValidator<CreateBookDto> validator, IMapper mapper, ILogger<CreateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<OperationResult<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateBookCommand for book title: {Title}", request.NewBook.Title);

            var validationResult = await _validator.ValidateAsync(request.NewBook, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                return OperationResult<BookDto>.Failure(errors, "Validation failed.");
            }

            var authorExists = await _authorRepository.GetByIdAsync(request.NewBook.AuthorId);
            if (authorExists == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", request.NewBook.AuthorId);
                return OperationResult<BookDto>.Failure($"Author with ID {request.NewBook.AuthorId} was not found.", "Error: Author not found.");
            }

            var newBook = _mapper.Map<Book>(request.NewBook);
            await _bookRepository.CreateAsync(newBook);
            _logger.LogInformation("Book created successfully: {BookId}, Title: {Title}", newBook.Id, newBook.Title);

            var createdBookDto = _mapper.Map<BookDto>(newBook);
            return OperationResult<BookDto>.Successful(createdBookDto, "Book created successfully.");
        }
    }
}
