using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IQueryRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;

        public GetAllBooksQueryHandler(IQueryRepository<Book> bookRepository, IMapper mapper, ILogger<GetAllBooksQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllBooksQuery");
            var books = await _bookRepository.GetAllAsync();

            _logger.LogInformation("{Count} books retrieved", books.Count());
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }
    }
}
