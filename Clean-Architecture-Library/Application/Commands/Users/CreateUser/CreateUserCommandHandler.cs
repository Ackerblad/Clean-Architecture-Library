using Application.DTOs.UserDtos;
using Application.Helpers;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserDto>>
    {
        private readonly ICommandRepository<User> _commandRepository;
        private readonly IQueryRepository<User> _queryRepository;
        private readonly IValidator<CreateUserDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(ICommandRepository<User> commandRepository, IQueryRepository<User> queryRepository,
                                        IValidator<CreateUserDto> validator, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateUserCommand for user: {Username}", request.NewUser.Username);

            var validationResult = await _validator.ValidateAsync(request.NewUser, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                return OperationResult<UserDto>.Failure(errors, "Validation failed.");
            }

            var existingUser = (await _queryRepository.GetAllAsync()).FirstOrDefault(user => user.Username == request.NewUser.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("User with username {Username} already exists.", request.NewUser.Username);
                return OperationResult<UserDto>.Failure($"User with username {request.NewUser.Username} already exists.", "Error: Duplicate username.");
            }

            var hashedPassword = PasswordHelper.HashPassword(request.NewUser.Password);
            request.NewUser.Password = hashedPassword;

            var newUser = _mapper.Map<User>(request.NewUser);
            await _commandRepository.CreateAsync(newUser);
            _logger.LogInformation("User created successfully: {UserId} {Username}", newUser.Id, newUser.Username);

            var createdUserDto = _mapper.Map<UserDto>(newUser);
            return OperationResult<UserDto>.Successful(createdUserDto, "User created successfully.");
        }
    }
}
