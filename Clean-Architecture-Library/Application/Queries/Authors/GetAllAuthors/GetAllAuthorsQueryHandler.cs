using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
    {
        private readonly IQueryRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IQueryRepository<Author> authorRepository, IMapper mapper, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllAuthorsQuery");
            var authors = await _authorRepository.GetAllAsync();

            _logger.LogInformation("{Count} authors retrieved", authors.Count());
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }
    }
}
