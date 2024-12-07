using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
    {
        private readonly IQueryRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "allAuthorsCacheKey";

        public GetAllAuthorsQueryHandler(IQueryRepository<Author> authorRepository, IMapper mapper, ILogger<GetAllAuthorsQueryHandler> logger, IMemoryCache memoryCache)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllAuthorsQuery");

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Author>? allAuthors))
            {
                _logger.LogInformation("Cache miss. Retrieving authors from database.");

                allAuthors = await _authorRepository.GetAllAsync();
                _memoryCache.Set(cacheKey, allAuthors, TimeSpan.FromMinutes(5));

                _logger.LogInformation("Author data cached successfully.");
            }

            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(allAuthors);
            _logger.LogInformation("{Count} authors retrieved", authorDtos.Count());

            return authorDtos;
        }
    }
}
