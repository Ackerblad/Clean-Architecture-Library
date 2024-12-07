using Application.Commands.Users.CreateUser;
using Application.DTOs.UserDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.UserTests.CommandTests
{
    public class CreateUserCommandHandlerTests
    {
        private Mock<ICommandRepository<User>> _mockCommandRepository;
        private Mock<IQueryRepository<User>> _mockQueryRepository;
        private Mock<IValidator<CreateUserDto>> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<CreateUserCommandHandler>> _mockLogger;
        private CreateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockCommandRepository = new Mock<ICommandRepository<User>>();
            _mockQueryRepository = new Mock<IQueryRepository<User>>();
            _mockValidator = new Mock<IValidator<CreateUserDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();

            _handler = new CreateUserCommandHandler(
                _mockCommandRepository.Object,
                _mockQueryRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_UserIsUnique_ReturnsSuccessOperationResult()
        {
            //Arrange
            var newUserDto = new CreateUserDto { Username = "uniqueUser", Password = "password123" };
            var command = new CreateUserCommand(newUserDto);
            var userEntity = new User();
            var userDto = new UserDto { Username = "uniqueUser" };

            _mockValidator.Setup(validator => validator.ValidateAsync(newUserDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockQueryRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(new List<User>());

            _mockMapper.Setup(mapper => mapper.Map<User>(newUserDto))
                       .Returns(userEntity);

            _mockMapper.Setup(mapper => mapper.Map<UserDto>(userEntity))
                       .Returns(userDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("User created successfully."));
            _mockCommandRepository.Verify(repo => repo.CreateAsync(userEntity), Times.Once);
        }

        [Test]
        public async Task Handle_UserIsNotUnique_ReturnsFailureOperationResult()
        {
            //Arrange
            var newUserDto = new CreateUserDto { Username = "duplicateUser" };
            var existingUsers = new List<User> { new User { Username = "duplicateUser" } };
            var command = new CreateUserCommand(newUserDto);

            _mockValidator.Setup(validator => validator.ValidateAsync(newUserDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockQueryRepository.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(existingUsers);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Duplicate username."));
            _mockCommandRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public async Task Handle_InvalidDto_ReturnsFailureOperationResult()
        {
            //Arrange
            var newUserDto = new CreateUserDto { Username = "" };
            var command = new CreateUserCommand(newUserDto);
            var validationErrors = new[] { new ValidationFailure("Username", "Username is required.") };

            _mockValidator.Setup(validator => validator.ValidateAsync(newUserDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationErrors));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("Username is required."));
            _mockCommandRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
