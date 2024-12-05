using Application.DTOs.AuthorDtos;
using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<AuthorDto>>
    {
        private readonly ICommandRepository<Author> _commandRepository;
        private readonly IQueryRepository<Author> _queryRepository;
        private readonly IValidator<UpdateAuthorDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(ICommandRepository<Author> commandRepository, IQueryRepository<Author> queryRepository,
                                          IValidator<UpdateAuthorDto> validator, IMapper mapper, ILogger<UpdateAuthorCommandHandler> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<AuthorDto>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateAuthorCommand for AuthorId: {AuthorId}", request.AuthorId);

            var validationResult = await _validator.ValidateAsync(request.UpdatedAuthor, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                return OperationResult<AuthorDto>.Failure(errors, "Validation failed.");
            }

            var existingAuthor = await _queryRepository.GetByIdAsync(request.AuthorId);
            if (existingAuthor == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", request.AuthorId);
                return OperationResult<AuthorDto>.Failure($"Author with ID {request.AuthorId} was not found.", "Error: Author not found.");
            }

            _mapper.Map(request.UpdatedAuthor, existingAuthor);
            await _commandRepository.UpdateAsync(existingAuthor);
            _logger.LogInformation("Author with ID {AuthorId} updated successfully.", request.AuthorId);

            var updatedAuthorDto = _mapper.Map<AuthorDto>(existingAuthor);
            return OperationResult<AuthorDto>.Successful(updatedAuthorDto, "Author updated successfully.");
        }
    }
}
