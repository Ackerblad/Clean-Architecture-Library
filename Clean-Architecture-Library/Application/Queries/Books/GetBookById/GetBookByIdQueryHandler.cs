using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<BookDto>>
    {
        private readonly IQueryRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBookByIdQueryHandler> _logger;

        public GetBookByIdQueryHandler(IQueryRepository<Book> bookRepository, IMapper mapper, ILogger<GetBookByIdQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetBookByIdQuery for BookId: {BookId}", request.BookId);

            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found.", request.BookId);
                return OperationResult<BookDto>.Failure($"Book with ID {request.BookId} was not found.", "Error: Book Not Found");
            }

            _logger.LogInformation("Book with ID {BookId} retrieved successfully.", request.BookId);

            var bookDto = _mapper.Map<BookDto>(book);
            return OperationResult<BookDto>.Successful(bookDto, "Book retrieved successfully.");
        }
    }
}
