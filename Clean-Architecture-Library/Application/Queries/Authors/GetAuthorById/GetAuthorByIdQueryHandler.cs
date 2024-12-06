using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<AuthorDto>>
    {
        private readonly IQueryRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IQueryRepository<Author> authorRepository, IMapper mapper, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAuthorByIdQuery for AuthorId: {AuthorId}", request.AuthorId);

            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", request.AuthorId);
                return OperationResult<AuthorDto>.Failure($"Author with ID {request.AuthorId} was not found.", "Error: Author Not Found");
            }

            _logger.LogInformation("Author with ID {AuthorId} retrieved successfully.", request.AuthorId);

            var authorDto = _mapper.Map<AuthorDto>(author);
            return OperationResult<AuthorDto>.Successful(authorDto, "Author retrieved successfully.");
        }
    }
}
