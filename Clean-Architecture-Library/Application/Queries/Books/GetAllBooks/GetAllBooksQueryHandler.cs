using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IQueryRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "allBooksCacheKey";

        public GetAllBooksQueryHandler(IQueryRepository<Book> bookRepository, IMapper mapper, ILogger<GetAllBooksQueryHandler> logger, IMemoryCache memoryCache)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllBooksQuery");

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Book>? allBooks))
            {
                _logger.LogInformation("Cache miss. Retrieving books from database.");

                allBooks = await _bookRepository.GetAllAsync();
                _memoryCache.Set(cacheKey, allBooks, TimeSpan.FromMinutes(5));

                _logger.LogInformation("Books data cached successfully.");
            }

            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(allBooks);
            _logger.LogInformation("{Count} books retrieved", bookDtos.Count());

            return bookDtos;
        }
    }
}
