using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<AuthorDto>>
    {
        private readonly ICommandRepository<Author> _authorRepository;
        private readonly IValidator<CreateAuthorDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAuthorCommandHandler> _logger;


        public CreateAuthorCommandHandler(ICommandRepository<Author> authorRepository, IValidator<CreateAuthorDto> validator,
                                          IMapper mapper, ILogger<CreateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateAuthorCommand for author: {FirstName} {LastName}", request.NewAuthor.FirstName, request.NewAuthor.LastName);

            var validationResult = await _validator.ValidateAsync(request.NewAuthor, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                return OperationResult<AuthorDto>.Failure(errors, "Validation failed.");
            }

            var newAuthor = _mapper.Map<Author>(request.NewAuthor);
            await _authorRepository.CreateAsync(newAuthor);
            _logger.LogInformation("Author created successfully: {AuthorId} {FirstName} {LastName}", newAuthor.Id, newAuthor.FirstName, newAuthor.LastName);

            var createdAuthorDto = _mapper.Map<AuthorDto>(newAuthor);
            return OperationResult<AuthorDto>.Successful(createdAuthorDto, "Author created successfully.");
        }
    }
}
